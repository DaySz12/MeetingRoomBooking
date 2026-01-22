using MeetingRoomBooking.Api.Interfaces;
using MeetingRoomBooking.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomBooking.Api.Controllers
{
    [ApiController]
    [Route("api/admin/rooms")]
    [Authorize(Roles = "ADMIN")]
    public class AdminRoomsController : ControllerBase
    {
        private readonly IAdminRoomService _service;

        public AdminRoomsController(IAdminRoomService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<room>>> GetAll()
        {
            return await _service.GetAllAsync();
        }


        [HttpPost]
        public async Task<IActionResult> Create(room model)
        {
            var ok = await _service.CreateAsync(model);
            if (!ok) return BadRequest("ชื่อห้องซ้ำ");
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, room model)
        {
            var ok = await _service.UpdateAsync(id, model);
            if (!ok) return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result.success)
                return BadRequest(result.error);

            return Ok();
        }
    }
}
