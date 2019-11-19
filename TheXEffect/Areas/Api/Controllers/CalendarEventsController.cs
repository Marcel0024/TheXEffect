using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TheXEffect.Areas.Api.Models;
using TheXEffect.Data;
using TheXEffect.Data.Extensions;
using TheXEffect.Data.Models;

namespace TheXEffect.Areas.Api.Controllers
{
    [Area("api")]
    [Route("[area]/[controller]")]
    [ApiController]
    [Authorize]
    public class CalendarEventsController : ControllerBase
    {
        private readonly TheXDbContext _dbContext;

        public CalendarEventsController(TheXDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromBody] EventVM model)
        {
            if (!DateTime.TryParseExact(model.Start, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime parsedDate))
            {
                return BadRequest();
            }

            var showCompliment = false;
            var calendarId = User.GetDefaultCalendarId();

            var calendarEvents = await _dbContext.CalendarEvents
                .Where(ce => ce.DateTime.Date == parsedDate.Date)
                .Where(ce => ce.CalendarId == calendarId)
                .ToListAsync();

            if (calendarEvents.Any())
            {
                _dbContext.RemoveRange(calendarEvents);
            }
            else
            {
                _dbContext.Add(new CalendarEvent
                {
                    DateTime = parsedDate,
                    Title = model.Title,
                    CalendarId = User.GetDefaultCalendarId()
                });

                if (parsedDate.Date == DateTime.Now.Date)
                {
                    showCompliment = true;
                }
            }

            await _dbContext.SaveChangesAsync();

            return new JsonResult(new { showCompliment });
        }

        [HttpPost("addmultiple")]
        public async Task<IActionResult> AddMultiple([FromBody] EventsVM model)
        {
            var calendarId = User.GetDefaultCalendarId();

            foreach (var eventVM in model.Events)
            {
                if (!DateTime.TryParseExact(eventVM.Start, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime parsedDate))
                {
                    return BadRequest();
                }

                // Case: User is logged out, adds events and than logs in - front end sends everything
                // Maybe it already exist
                if (await _dbContext.CalendarEvents
                    .Where(ce => ce.DateTime.Date == parsedDate.Date)
                    .Where(ce => ce.CalendarId == calendarId)
                    .AnyAsync())
                {
                    continue;
                }

                _dbContext.CalendarEvents.Add(new CalendarEvent
                {
                    DateTime = parsedDate,
                    Title = eventVM.Title,
                    CalendarId = calendarId
                });
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}