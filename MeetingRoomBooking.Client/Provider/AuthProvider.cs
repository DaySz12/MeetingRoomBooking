using MeetingRoomBooking.Api.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace MeetingRoomBooking.Client.Providers
{
    public class AuthProvider
    {
        private readonly HttpClient _http;

        public AuthProvider(HttpClient http)
        {
            _http = http;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var res = await _http.PostAsJsonAsync("api/auth/login", request);
            if (!res.IsSuccessStatusCode) return null;
            return await res.Content.ReadFromJsonAsync<LoginResponse>();
        }


        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var res = await _http.PostAsJsonAsync("api/auth/register", request);
            return res.IsSuccessStatusCode;
        }
    }
}
