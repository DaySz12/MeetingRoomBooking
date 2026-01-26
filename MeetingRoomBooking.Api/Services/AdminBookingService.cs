using MeetingRoomBooking.Api.Context;
using MeetingRoomBooking.Api.Models;
using Microsoft.EntityFrameworkCore;

public class AdminBookingService : IAdminBookingService
{
    private readonly AppDbContext _context;

    public AdminBookingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AdminBookingSummaryDto> GetSummaryAsync()
    {
        var today = DateTime.Today;

        return new AdminBookingSummaryDto
        {
            TotalBookings = await _context.bookings.CountAsync(),
            TodayBookings = await _context.bookings
                .CountAsync(b => b.start_time.Date == today),
            TotalRooms = await _context.rooms.CountAsync()
        };
    }

    public async Task<List<AdminBookingDto>> GetAllBookingsAsync()
    {
        return await _context.bookings
            .Include(b => b.room)
            .Include(b => b.user)
            .OrderByDescending(b => b.start_time)
            .Select(b => new AdminBookingDto
            {
                Id = b.id,
                RoomName = b.room.name,
                Username = b.user.username,
                Title = b.title,
                StartTime = b.start_time,
                EndTime = b.end_time
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteBookingAsync(int id)
    {
        var booking = await _context.bookings.FindAsync(id);
        if (booking == null) return false;

        _context.bookings.Remove(booking);
        await _context.SaveChangesAsync();
        return true;
    }
}
