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
    public class TimeSheetController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TimeSheetController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TimeSheet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeSheetMaster>>> GetTimeSheets()
        {
            return await _context.TimeSheets
                .Include(t => t.Employee)
                .Include(t => t.Project)
                .ToListAsync();
        }

        // GET: api/TimeSheet/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeSheetMaster>> GetTimeSheet(int id)
        {
            var timeSheet = await _context.TimeSheets
                .Include(t => t.Employee)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (timeSheet == null)
                return NotFound();

            return timeSheet;
        }

        // POST: api/TimeSheet
        [HttpPost]
        public async Task<ActionResult<TimeSheetMaster>> PostTimeSheet(TimeSheetMaster timeSheet)
        {
            _context.TimeSheets.Add(timeSheet);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTimeSheet), new { id = timeSheet.Id }, timeSheet);
        }

        // PUT: api/TimeSheet/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimeSheet(int id, TimeSheetMaster timeSheet)
        {
            if (id != timeSheet.Id)
                return BadRequest();

            _context.Entry(timeSheet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TimeSheets.Any(t => t.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/TimeSheet/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeSheet(int id)
        {
            var timeSheet = await _context.TimeSheets.FindAsync(id);
            if (timeSheet == null)
                return NotFound();

            _context.TimeSheets.Remove(timeSheet);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
