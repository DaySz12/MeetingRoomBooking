using MeetingRoomBooking.Api.Context;
using MeetingRoomBooking.Api.Interfaces;
using MeetingRoomBooking.Api.Models;
using Microsoft.EntityFrameworkCore;

public class BookingService : IBookingService
{
    private readonly AppDbContext _context;

    public BookingService(AppDbContext context)
    {
        _context = context;
    }

    // 🔹 จองห้อง
    public async Task CreateAsync(CreateBookingDto dto)
    {
        if (dto.StartTime >= dto.EndTime)
            throw new Exception("เวลาเริ่มต้องน้อยกว่าเวลาจบ");

        var isOverlap = await _context.bookings
            .AnyAsync(b =>
                b.room_id == dto.RoomId &&
                dto.StartTime < b.end_time &&
                dto.EndTime > b.start_time
            );

        if (isOverlap)
            throw new Exception("ช่วงเวลานี้มีการจองแล้ว");

        var booking = new booking
        {
            room_id = dto.RoomId,
            user_id = dto.UserId,
            start_time = dto.StartTime,
            end_time = dto.EndTime,
            title = dto.Title,
            note = dto.Note
        };

        await _context.bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    // 🔹 แก้ไขการจอง (เฉพาะของตัวเอง)
    public async Task UpdateAsync(int bookingId, int userId, UpdateBookingDto dto)
    {
        if (dto.StartTime >= dto.EndTime)
            throw new Exception("เวลาเริ่มต้องน้อยกว่าเวลาจบ");

        var booking = await _context.bookings
            .FirstOrDefaultAsync(b => b.id == bookingId && b.user_id == userId);

        if (booking == null)
            throw new Exception("ไม่พบการจอง");

        var isOverlap = await _context.bookings
            .AnyAsync(b =>
                b.room_id == booking.room_id &&
                b.id != bookingId &&
                dto.StartTime < b.end_time &&
                dto.EndTime > b.start_time
            );

        if (isOverlap)
            throw new Exception("ช่วงเวลานี้มีการจองแล้ว");

        booking.start_time = dto.StartTime;
        booking.end_time = dto.EndTime;
        booking.title = dto.Title;
        booking.note = dto.Note;

        await _context.SaveChangesAsync();
    }

    // 🔹 ยกเลิกการจอง (เฉพาะของตัวเอง)
    public async Task DeleteAsync(int bookingId, int userId)
    {
        var booking = await _context.bookings
            .FirstOrDefaultAsync(b => b.id == bookingId && b.user_id == userId);

        if (booking == null)
            throw new Exception("ไม่พบการจอง");

        _context.bookings.Remove(booking);
        await _context.SaveChangesAsync();
    }

    // 🔹 ดูการจองของฉัน
    public async Task<List<BookingDto>> GetMybookingsAsync(int userId)
    {
        return await _context.bookings
            .Where(b => b.user_id == userId)
            .OrderByDescending(b => b.start_time)
            .Select(b => new BookingDto
            {
                Id = b.id,
                RoomName = b.room.name,
                StartTime = b.start_time,
                EndTime = b.end_time,
                Title = b.title
            })
            .ToListAsync();
    }
}
