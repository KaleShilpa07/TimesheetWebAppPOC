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
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Project
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectMaster>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Project/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectMaster>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound($"Project with ID {id} not found.");

            return project;
        }

        // POST: api/Project
        [HttpPost]
        public async Task<ActionResult<ProjectMaster>> PostProject(ProjectMaster project)
        {
            try
            {
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProject), new { id = project.ProjectID }, project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving project: {ex.Message}");
            }
        }

        // PUT: api/Project/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, ProjectMaster project)
        {

            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject == null)
                return NotFound($"Project with ID {id} not found.");

            _context.Entry(existingProject).CurrentValues.SetValues(project);

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating project: {ex.Message}");
            }
        }

        // DELETE: api/Project/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound($"Project with ID {id} not found.");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
