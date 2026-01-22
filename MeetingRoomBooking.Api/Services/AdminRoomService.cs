using MeetingRoomBooking.Api.Context;
using MeetingRoomBooking.Api.Interfaces;
using MeetingRoomBooking.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomBooking.Api.Services
{
    public class AdminRoomService : IAdminRoomService
    {
        private readonly AppDbContext _context;

        public AdminRoomService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<room>> GetAllAsync()
        {
            return await _context.rooms
                .OrderBy(r => r.name)
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(room model)
        {
            var exists = await _context.rooms
                .AnyAsync(r => r.name == model.name);

            if (exists) return false;

            _context.rooms.Add(model);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(int id, room model)
        {
            var room = await _context.rooms.FindAsync(id);
            if (room == null) return false;

            room.name = model.name;
            room.capacity = model.capacity;
            room.equipment = model.equipment;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool success, string? error)> DeleteAsync(int id)
        {
            var hasFutureBooking = await _context.bookings
                .AnyAsync(b => b.room_id == id && b.start_time > DateTime.Now);

            if (hasFutureBooking)
                return (false, "มีการจองในอนาคต ต้องยกเลิกก่อน");

            var room = await _context.rooms.FindAsync(id);
            if (room == null)
                return (false, "ไม่พบห้อง");

            _context.rooms.Remove(room);
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}
