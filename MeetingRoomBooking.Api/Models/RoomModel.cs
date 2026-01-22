namespace MeetingRoomBooking.Api.Models
{
    public class RoomModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? Equipment { get; set; }
    }
}
