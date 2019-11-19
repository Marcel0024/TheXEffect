using System;

namespace TheXEffect.Data.Models
{
    public class CalendarEvent
    {
        public int Id { get; set; }

        public Guid? CalendarId { get; set; }

        public Calendar Calendar { get; set; }

        public DateTime DateTime { get; set; }

        public string Title { get; set; }
    }
}