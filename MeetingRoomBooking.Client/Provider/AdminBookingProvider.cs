using MeetingRoomBooking.Api.Models;
using System.Net.Http.Json;

public class AdminBookingProvider
{
    private readonly HttpClient _http;

    public AdminBookingProvider(HttpClient http)
    {
        _http = http;
    }

    public async Task<AdminBookingSummaryDto?> GetSummaryAsync()
        => await _http.GetFromJsonAsync<AdminBookingSummaryDto>(
            "api/admin/bookings/summary");

    public async Task<List<AdminBookingDto>?> GetBookingsAsync()
        => await _http.GetFromJsonAsync<List<AdminBookingDto>>(
            "api/admin/bookings");

    public async Task<bool> DeleteAsync(int id)
        => (await _http.DeleteAsync($"api/admin/bookings/{id}"))
            .IsSuccessStatusCode;
}
