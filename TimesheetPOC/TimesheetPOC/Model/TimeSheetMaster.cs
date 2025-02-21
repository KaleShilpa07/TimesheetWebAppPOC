using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimesheetPOC.Model
{
    public class TimeSheetMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EmployeeID { get; set; }
        public string ProjectID { get; set; }
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }

        public EmployeeMaster Employee { get; set; }
        public ProjectMaster Project { get; set; }
    }
}

