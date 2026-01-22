using MeetingRoomBooking.Api.Models;

namespace MeetingRoomBooking.Api.Interfaces
{
    public interface IBookingService
    {
        Task CreateAsync(CreateBookingDto dto);
        Task UpdateAsync(int bookingId, int userId, UpdateBookingDto dto);
        Task DeleteAsync(int bookingId, int userId);
        Task<List<BookingDto>> GetMybookingsAsync(int userId);
    }

}
