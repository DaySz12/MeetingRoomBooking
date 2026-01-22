using MeetingRoomBooking.Api.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace MeetingRoomBooking.Client.Providers
{
    public class RoomProvider
    {
        private readonly HttpClient _http;

        public RoomProvider(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<RoomModel>?> GetRoomsAsync()
        {
            return await _http.GetFromJsonAsync<List<RoomModel>>("api/rooms");
        }

        public async Task<bool> CreateAsync(RoomModel model)
        {
            var res = await _http.PostAsJsonAsync("api/rooms", model);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, RoomModel model)
        {
            var res = await _http.PutAsJsonAsync($"api/rooms/{id}", model);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var res = await _http.DeleteAsync($"api/rooms/{id}");
            return res.IsSuccessStatusCode;
        }
    }
}
