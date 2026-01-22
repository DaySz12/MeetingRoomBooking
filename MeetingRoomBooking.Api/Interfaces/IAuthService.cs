using MeetingRoomBooking.Api.Models;

namespace MeetingRoomBooking.Api.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request); 
        Task<bool> RegisterAsync(RegisterRequest request);    
    }

}
