using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TimesheetPOC.Data;
using TimesheetPOC.Model;

[Route("api/[controller]")]
[ApiController]
public class ExcelImportController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExcelImportController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File not selected.");
        }

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        try
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    if (result.Tables.Count == 0)
                    {
                        return BadRequest("Empty Excel file.");
                    }

                    var timesheetTable = result.Tables[0];

                    // Validate required columns
                    var requiredColumns = new[] {
                        "LegalEntity", "EmployeeID", "EmployeeName", "ProjectID", "ProjectName",
                        "CustomerName", "ProjectStage", "Category", "Total"
                    };

                    foreach (var column in requiredColumns)
                    {
                        if (!timesheetTable.Columns.Contains(column))
                        {
                            return BadRequest($"Missing required column: {column}");
                        }
                    }

                    // Process timesheet data
                    await ProcessTimesheetTable(timesheetTable);
                }
            }

            return Ok("File uploaded and processed successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private async Task ProcessTimesheetTable(DataTable table)
    {
        var timesheetsToAdd = new List<TimeSheetMaster>();
        var employeesToAdd = new List<EmployeeMaster>();
        var projectsToAdd = new List<ProjectMaster>();

        var existingEmployees = _context.Employees.AsNoTracking().ToDictionary(e => e.EmployeeID);
        var existingProjects = _context.Projects.AsNoTracking().ToDictionary(p => p.ProjectID);
        var existingTimesheets = _context.TimeSheets.AsNoTracking()
            .Select(t => new { t.ProjectID, t.EmployeeID, t.Date })
            .ToHashSet();

        // Identify date columns dynamically
        List<string> dateColumns = table.Columns
            .Cast<DataColumn>()
            .Select(col => col.ColumnName)
            .Where(col => ConvertHeaderToDate(col) != null)
            .ToList();

        foreach (DataRow row in table.Rows)
        {
            string employeeId = row["EmployeeID"].ToString();
            string employeeName = row["EmployeeName"].ToString();
            string legalEntity = row["LegalEntity"].ToString();

            if (string.IsNullOrEmpty(employeeId)) continue;

            // Add Employee if not exists
            if (!existingEmployees.ContainsKey(employeeId))
            {
                var employee = new EmployeeMaster
                {
                    EmployeeID = employeeId,
                    EmployeeName = employeeName,
                    LegalEntity = legalEntity,
                    Status = "Active"
                };
                employeesToAdd.Add(employee);
                existingEmployees[employeeId] = employee;
            }

            string projectId = row["ProjectID"].ToString();
            string projectName = row["ProjectName"].ToString();
            string customerName = row["CustomerName"].ToString();
            string projectStage = row["ProjectStage"].ToString();
            string category = row["Category"].ToString();

            // Add Project if not exists
            if (!existingProjects.ContainsKey(projectId))
            {
                var project = new ProjectMaster
                {
                    ProjectID = projectId,
                    ProjectName = projectName,
                    CustomerName = customerName,
                    ProjectStage = projectStage,
                    Category = category,
                    Status = "Active"
                };
                projectsToAdd.Add(project);
                existingProjects[projectId] = project;
            }

            // Process each date column and add timesheet entries
            foreach (string dateColumn in dateColumns)
            {
                if (row[dateColumn] != DBNull.Value && decimal.TryParse(row[dateColumn].ToString(), out decimal hours))
                {
                    DateTime? timesheetDate = ConvertHeaderToDate(dateColumn);

                    if (timesheetDate.HasValue)
                    {
                        // Prevent duplicate timesheet entry
                        var timesheetKey = new { ProjectID = projectId, EmployeeID = employeeId, Date = timesheetDate.Value };
                        if (!existingTimesheets.Contains(timesheetKey))
                        {
                            timesheetsToAdd.Add(new TimeSheetMaster
                            {
                                EmployeeID = employeeId,
                                ProjectID = projectId,
                                Date = timesheetDate.Value,
                                Hours = hours
                            });

                            existingTimesheets.Add(timesheetKey); // Update in-memory set
                            Console.WriteLine($"Added Timesheet: {employeeId} - {projectId} - {timesheetDate.Value:yyyy-MM-dd} - {hours} hrs");
                        }
                        else
                        {
                            Console.WriteLine($"Skipping duplicate entry: {employeeId} - {projectId} - {timesheetDate.Value:yyyy-MM-dd}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Skipping invalid date column format: {dateColumn}");
                    }
                }
            }
        }

        // Batch insert data for efficiency
        if (employeesToAdd.Any()) await _context.Employees.AddRangeAsync(employeesToAdd);
        if (projectsToAdd.Any()) await _context.Projects.AddRangeAsync(projectsToAdd);
        if (timesheetsToAdd.Any()) await _context.TimeSheets.AddRangeAsync(timesheetsToAdd);

        await _context.SaveChangesAsync();
    }

    private DateTime? ConvertHeaderToDate(string columnHeader)
    {
        try
        {
            string[] parts = columnHeader.Split(' ');
            if (parts.Length != 2) return null; // Ensure correct format

            string datePart = parts[1]; // Extract "12/29" or "01/01"
            string[] dateSplit = datePart.Split('/');

            if (dateSplit.Length != 2) return null; // Ensure correct date format

            int month = int.Parse(dateSplit[0]); // Extract month
            int day = int.Parse(dateSplit[1]); // Extract day

            int currentYear = DateTime.Now.Year;
            int previousYear = currentYear - 1;

            // If the month is January, assign it to the previous year
            int year = (month == 1) ? previousYear : currentYear;

            return new DateTime(year, month, day);
        }
        catch
        {
            return null; // Return null if parsing fails
        }
    }
}
