using System;
using System.Collections.Generic;

namespace MeetingRoomBooking.Api.Models
{
    public partial class booking
    {
        public int id { get; set; }
        public int room_id { get; set; }
        public int user_id { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public string title { get; set; } = null!;
        public string? note { get; set; }
        public DateTime? created_at { get; set; }

        public virtual room room { get; set; } = null!;
        public virtual user user { get; set; } = null!;
    }
}
