using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimesheetPOC.Model
{
    [Table("ProjectMaster")] 
    public class ProjectMaster
    {
        [Key]
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string ProjectStage { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public ICollection<TimeSheetMaster> TimeSheets { get; set; }
    }
}
