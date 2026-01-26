namespace MeetingRoomBooking.Api.Models;

public class AdminBookingDto
{
    public int Id { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
