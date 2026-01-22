using MeetingRoomBooking.Api.Models;
using System.Net.Http.Json;
using System.Net.Http.Headers; // 👈 1. ต้องเพิ่มบรรทัดนี้
using Microsoft.JSInterop;     // 👈 2. ต้องเพิ่มบรรทัดนี้

namespace MeetingRoomBooking.Client.Providers
{
    public class AdminRoomProvider
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js; // 👈 3. เพิ่มตัวช่วยคุยกับ JS

        // 4. Inject IJSRuntime เข้ามาใน Constructor
        public AdminRoomProvider(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        // 5. สร้างฟังก์ชันช่วยแนบ Token
        private async Task AddJwtHeader()
        {
            try
            {
                var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");
                if (!string.IsNullOrEmpty(token))
                {
                    _http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch { /* กัน Error ตอน Prerender */ }
        }

        // GET: ห้องทั้งหมด
        public async Task<List<room>> GetRoomsAsync()
        {
            await AddJwtHeader(); // 👈 6. เรียกใช้ก่อนยิง API ทุกครั้ง
            return await _http.GetFromJsonAsync<List<room>>("api/admin/rooms") ?? new();
        }

        // POST: เพิ่มห้อง
        public async Task<bool> CreateAsync(room model)
        {
            await AddJwtHeader(); // 👈 เรียกใช้
            var res = await _http.PostAsJsonAsync("api/admin/rooms", model);
            return res.IsSuccessStatusCode;
        }

        // PUT: แก้ไขห้อง
        public async Task<bool> UpdateAsync(room model)
        {
            await AddJwtHeader(); // 👈 เรียกใช้
            var res = await _http.PutAsJsonAsync($"api/admin/rooms/{model.id}", model);
            return res.IsSuccessStatusCode;
        }

        // DELETE: ลบห้อง
        public async Task<(bool success, string? error)> DeleteAsync(int id)
        {
            await AddJwtHeader(); // 👈 เรียกใช้
            var res = await _http.DeleteAsync($"api/admin/rooms/{id}");

            if (res.IsSuccessStatusCode)
                return (true, null);

            var error = await res.Content.ReadAsStringAsync();
            return (false, error);
        }
    }
}