using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TheXEffect.Data.Models
{
    public class User : IdentityUser
    {
        public DateTime DateTimeCreated { get; set; }

        public DateTime DateTimeLastLoggedIn { get; set; }

        public ICollection<Calendar> Calendars { get; set; } = new HashSet<Calendar>();
    }
}
