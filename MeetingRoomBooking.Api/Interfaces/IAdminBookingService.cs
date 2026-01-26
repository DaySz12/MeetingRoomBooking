using MeetingRoomBooking.Api.Models;

public interface IAdminBookingService
{
    Task<AdminBookingSummaryDto> GetSummaryAsync();
    Task<List<AdminBookingDto>> GetAllBookingsAsync();
    Task<bool> DeleteBookingAsync(int id);
}
