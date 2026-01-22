using Microsoft.AspNetCore.Authorization; // 👈 1. ต้องมี
using Microsoft.AspNetCore.Mvc;
using MeetingRoomBooking.Api.Interfaces;
using MeetingRoomBooking.Api.Models;
using System.Security.Claims; // 👈 2. ต้องมี (ไว้แกะ Token)

namespace MeetingRoomBooking.Api.Controllers // 👈 ใส่ Namespace ให้ตรงกับโปรเจกต์
{
    [ApiController]
    [Route("api/bookings")]
    [Authorize] // 👈 3. บังคับ Login (ถ้าไม่มี Token จะเข้าไม่ได้)
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingsController(IBookingService service)
        {
            _service = service;
        }

        // ฟังก์ชันช่วยแกะ User ID จาก Token (ใช้ซ้ำได้สะดวก)
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("id")?.Value ??
                              User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("ไม่พบข้อมูล User ID ใน Token");
            }

            return int.Parse(userIdClaim);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            try
            {
                // 👇 ยัด User ID จาก Token ใส่ DTO ก่อนส่งให้ Service
                dto.UserId = GetCurrentUserId();

                await _service.CreateAsync(dto);
                return Ok(new { message = "จองห้องสำเร็จ" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBookingDto dto)
        {
            // ลบ int userId ออกจาก Parameter แล้วใช้ตัวนี้แทน 👇
            int userId = GetCurrentUserId();

            try
            {
                await _service.UpdateAsync(id, userId, dto);
                return Ok(new { message = "แก้ไขการจองเรียบร้อย" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // ลบ int userId ออกจาก Parameter แล้วใช้ตัวนี้แทน 👇
            int userId = GetCurrentUserId();

            try
            {
                await _service.DeleteAsync(id, userId);
                return Ok(new { message = "ยกเลิกการจองเรียบร้อย" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my")]
        public async Task<IActionResult> MyBookings()
        {
            // ลบ int userId ออกจาก Parameter แล้วใช้ตัวนี้แทน 👇
            int userId = GetCurrentUserId();
            return Ok(await _service.GetMybookingsAsync(userId));
        }
    }
}