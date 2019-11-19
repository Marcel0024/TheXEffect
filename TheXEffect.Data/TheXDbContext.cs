using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheXEffect.Data.Models;

namespace TheXEffect.Data
{
    public class TheXDbContext : IdentityDbContext
    {
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<CalendarEvent> CalendarEvents { get; set; }

        public TheXDbContext(DbContextOptions<TheXDbContext> options)
            : base(options)
        {
        }
    }
}
