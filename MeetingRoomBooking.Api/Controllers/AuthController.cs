using MeetingRoomBooking.Api.Interfaces;
using MeetingRoomBooking.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var result = await _service.LoginAsync(request);

        // ✅ ถ้า Service ส่ง null มา แปลว่า Login พลาด
        if (result == null)
        {
            return BadRequest("Username หรือ Password ไม่ถูกต้อง");
        }

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var success = await _service.RegisterAsync(request);

        // ✅ ถ้า Service ส่ง false มา แปลว่าชื่อซ้ำ
        if (!success)
        {
            return BadRequest("Username นี้ถูกใช้งานแล้ว");
        }

        return Ok(new { message = "สมัครสมาชิกสำเร็จ" });
    }

}
