using System;
using System.Collections.Generic;

namespace MeetingRoomBooking.Api.Models
{
    public partial class room
    {
        public room()
        {
            bookings = new HashSet<booking>();
        }

        public int id { get; set; }
        public string name { get; set; } = null!;
        public int capacity { get; set; }
        public string? equipment { get; set; }

        public virtual ICollection<booking> bookings { get; set; }
    }
}
