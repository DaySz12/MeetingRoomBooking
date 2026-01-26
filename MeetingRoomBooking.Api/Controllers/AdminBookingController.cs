using MeetingRoomBooking.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/admin/bookings")]
[Authorize(Roles = "ADMIN")]
public class AdminBookingController : ControllerBase
{
    private readonly IAdminBookingService _service;

    public AdminBookingController(IAdminBookingService service)
    {
        _service = service;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Summary()
        => Ok(await _service.GetSummaryAsync());

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllBookingsAsync());

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => await _service.DeleteBookingAsync(id)
            ? Ok()
            : NotFound();
}
