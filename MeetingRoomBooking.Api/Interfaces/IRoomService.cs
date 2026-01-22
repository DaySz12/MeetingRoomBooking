using MeetingRoomBooking.Api.Models;

namespace MeetingRoomBooking.Api.Interfaces
{
    public interface IRoomService
    {
        Task<List<RoomModel>> GetAllAsync();
        Task<RoomModel> CreateAsync(RoomModel dto);
        Task UpdateAsync(int id, RoomModel dto);
        Task DeleteAsync(int id);
    }

}
