namespace MeetingRoomBooking.Api.Models
{
    public class UpdateBookingDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Note { get; set; }
    }

}
