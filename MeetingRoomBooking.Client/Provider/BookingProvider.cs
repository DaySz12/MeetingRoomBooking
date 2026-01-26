using MeetingRoomBooking.Api.Models;
using System.Net.Http.Json;
using System.Net.Http.Headers; 
using Microsoft.JSInterop;    

namespace MeetingRoomBooking.Client.Providers
{
    public class BookingProvider
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js; 

        public BookingProvider(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }


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

        // ✅ CREATE: จองห้อง
        public async Task<bool> CreateBookingAsync(CreateBookingDto model) // เปลี่ยนชื่อให้ตรงกับที่หน้า UI เรียกใช้
        {
            await AddJwtHeader(); 
            var res = await _http.PostAsJsonAsync("api/bookings", model);

            // 2. ถ้าไม่สำเร็จ (เช่น 400 Bad Request หรือเวลาชน)
            if (!res.IsSuccessStatusCode)
            {
                // อ่านข้อความ Error จาก Backend (เช่น "ช่วงเวลานี้มีการจองแล้ว")
                var errorMsg = await res.Content.ReadAsStringAsync();

                // พยายามแกะ JSON ถ้าทำได้
                try
                {
                    var json = System.Text.Json.JsonDocument.Parse(errorMsg);
                    if (json.RootElement.TryGetProperty("message", out var msg))
                    {
                        throw new Exception(msg.GetString());
                    }
                }
                catch { }

                // ถ้าแกะไม่ได้ หรือเป็น Text ธรรมดา ก็โยนออกไปเลย
                throw new Exception(errorMsg.Replace("\"", ""));
            }

            return true;
        }

        // ✅ GET MY BOOKINGS: ดึงประวัติ
        public async Task<List<BookingDto>> GetMyBookingsAsync()
        {
            await AddJwtHeader(); 
            return await _http.GetFromJsonAsync<List<BookingDto>>("api/bookings/my") ?? new();
        }

        // ✅ DELETE: ยกเลิกจอง
        public async Task<bool> DeleteBookingAsync(int id)
        {
            await AddJwtHeader();
            var res = await _http.DeleteAsync($"api/bookings/{id}");
            return res.IsSuccessStatusCode;
        }
    }
}