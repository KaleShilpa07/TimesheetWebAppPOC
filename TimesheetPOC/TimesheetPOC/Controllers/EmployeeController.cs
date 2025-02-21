using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimesheetPOC.Data;
using TimesheetPOC.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimesheetPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeMaster>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employee/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeMaster>> GetEmployee(string id) // Ensure EmployeeID type matches the database
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound($"Employee with ID {id} not found.");

            return employee;
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<EmployeeMaster>> PostEmployee(EmployeeMaster employee)
        {
            try
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeID }, employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving data: {ex.Message}");
            }
        }

        // PUT: api/Employee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(string id, EmployeeMaster employee)
        {
            if (id != employee.EmployeeID)
                return BadRequest("Employee ID mismatch.");

            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
                return NotFound($"Employee with ID {id} not found.");

            _context.Entry(existingEmployee).CurrentValues.SetValues(employee);

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating data: {ex.Message}");
            }
        }

        // DELETE: api/Employee/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound($"Employee with ID {id} not found.");

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
