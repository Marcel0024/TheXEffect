using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TheXEffect.Data.Extensions;

namespace TheXEffect.Areas.Api.Controllers
{
    [Area("api")]
    [Route("[area]/[controller]")]
    [ApiController]
    [Authorize]
    public class CalendarController : ControllerBase
    {
        private readonly Data.TheXDbContext _dbContext;

        public CalendarController(Data.TheXDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetEvents")]
        public async Task<IActionResult> GetEvents(string start, string end)
        {
            var startDate = DateTime.Parse(start, null, System.Globalization.DateTimeStyles.RoundtripKind);
            var endDate = DateTime.Parse(end, null, System.Globalization.DateTimeStyles.RoundtripKind);

            var calendarId = User.GetDefaultCalendarId();

            return new JsonResult((await _dbContext.CalendarEvents
                    .Where(ce => ce.CalendarId == calendarId)
                    .Where(ce => ce.DateTime >= startDate)
                    .Where(ce => ce.DateTime <= endDate)
                    .Select(ce => new
                    {
                        ce.DateTime,
                        ce.Title
                    })
                    .ToListAsync())
                    .Select(ce => new
                    {
                        title = ce.Title,
                        start = ce.DateTime.ToString("yyyy-MM-dd")
                    })
                    .ToArray()
            );
        }
    }
}