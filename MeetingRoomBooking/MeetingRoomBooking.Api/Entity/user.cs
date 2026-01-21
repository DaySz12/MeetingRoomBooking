using System;
using System.Collections.Generic;

namespace MeetingRoomBooking.Api.Models
{
    public partial class user
    {
        public user()
        {
            bookings = new HashSet<booking>();
        }

        public int id { get; set; }
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
        public string role { get; set; } = null!;
        public DateTime? created_at { get; set; }

        public virtual ICollection<booking> bookings { get; set; }
    }
}
