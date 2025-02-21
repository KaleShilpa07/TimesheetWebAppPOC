using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimesheetPOC.Model
{
    public class EmployeeMaster
    {
        [Key]
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string LegalEntity { get; set; }
        public string Status { get; set; }
        public ICollection<TimeSheetMaster> TimeSheets { get; set; }
    
    }
}
