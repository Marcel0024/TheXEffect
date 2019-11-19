using System;
using System.Collections.Generic;

namespace TheXEffect.Data.Models
{
    public class Calendar
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string Title { get; set; }

        public DateTime DateTimeCreated { get; set; }

        public ICollection<CalendarEvent> CalendarEntries { get; set; } = new HashSet<CalendarEvent>();
    }
}
