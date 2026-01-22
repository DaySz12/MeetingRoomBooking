using MeetingRoomBooking.Api.Models;

public interface IAdminRoomService
{
    Task<List<room>> GetAllAsync();
    Task<bool> CreateAsync(room model);
    Task<bool> UpdateAsync(int id, room model);
    Task<(bool success, string? error)> DeleteAsync(int id);
}
