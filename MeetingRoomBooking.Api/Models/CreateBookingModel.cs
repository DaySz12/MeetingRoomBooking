namespace MeetingRoomBooking.Api.Models
{
    public class CreateBookingDto
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }   
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Note { get; set; }
    }

}
