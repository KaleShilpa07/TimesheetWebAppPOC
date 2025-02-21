using Microsoft.EntityFrameworkCore;
using TimesheetPOC.Model;

namespace TimesheetPOC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<EmployeeMaster> Employees { get; set; }
        public DbSet<ProjectMaster> Projects { get; set; }
        public DbSet<TimeSheetMaster> TimeSheets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeSheetMaster>()
                .HasOne(t => t.Employee)
                .WithMany(e => e.TimeSheets)
                .HasForeignKey(t => t.EmployeeID);

            modelBuilder.Entity<TimeSheetMaster>()
                .HasOne(t => t.Project)
                .WithMany(p => p.TimeSheets)
                .HasForeignKey(t => t.ProjectID);
        }
    }
}
