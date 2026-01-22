using MeetingRoomBooking.Api.Context;
using MeetingRoomBooking.Api.Interfaces;
using MeetingRoomBooking.Api.Models;
using Microsoft.EntityFrameworkCore;

public class RoomService : IRoomService
{
    private readonly AppDbContext _context;

    public RoomService(AppDbContext context)
    {
        _context = context;
    }

    // 🔹 ดูห้องทั้งหมด
    public async Task<List<RoomModel>> GetAllAsync()
    {
        return await _context.rooms
            .OrderBy(r => r.name)
            .Select(r => new RoomModel
            {
                Id = r.id,
                Name = r.name,
                Capacity = r.capacity,
                Equipment = r.equipment
            })
            .ToListAsync();
    }

    // 🔹 เพิ่มห้อง
    public async Task<RoomModel> CreateAsync(RoomModel dto)
    {
        var isDuplicate = await _context.rooms
            .AnyAsync(r => r.name == dto.Name);

        if (isDuplicate)
            throw new Exception("ชื่อห้องซ้ำ");

        var room = new room
        {
            name = dto.Name,
            capacity = dto.Capacity,
            equipment = dto.Equipment
        };

        await _context.rooms.AddAsync(room);
        await _context.SaveChangesAsync();

        dto.Id = room.id;
        return dto;
    }

    // 🔹 แก้ไขห้อง
    public async Task UpdateAsync(int id, RoomModel dto)
    {
        var room = await _context.rooms
            .FirstOrDefaultAsync(r => r.id == id);

        if (room == null)
            throw new Exception("ไม่พบห้อง");

        var isDuplicate = await _context.rooms
            .AnyAsync(r => r.name == dto.Name && r.id != id);

        if (isDuplicate)
            throw new Exception("ชื่อห้องซ้ำ");

        room.name = dto.Name;
        room.capacity = dto.Capacity;
        room.equipment = dto.Equipment;

        await _context.SaveChangesAsync();
    }

    // 🔹 ลบห้อง
    public async Task DeleteAsync(int id)
    {
        var hasFutureBooking = await _context.bookings
            .AnyAsync(b =>
                b.room_id == id &&
                b.start_time >= DateTime.Now
            );

        if (hasFutureBooking)
            throw new Exception("ไม่สามารถลบห้องได้ เนื่องจากมีการจองในอนาคต");

        var room = await _context.rooms
            .FirstOrDefaultAsync(r => r.id == id);

        if (room == null)
            throw new Exception("ไม่พบห้อง");

        _context.rooms.Remove(room);
        await _context.SaveChangesAsync();
    }
}
