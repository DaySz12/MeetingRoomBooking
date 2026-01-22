namespace MeetingRoomBooking.Api.Models
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; } = string.Empty;
    }

}
