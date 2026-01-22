using MeetingRoomBooking.Api.Context;
using MeetingRoomBooking.Api.Interfaces;
using MeetingRoomBooking.Api.Models;
using Microsoft.EntityFrameworkCore;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtTokenService _jwt;

    public AuthService(AppDbContext context, JwtTokenService jwt)
    {
        _context = context;
        _jwt = jwt;
    }

    // ✅ เปลี่ยนให้คืนค่าเป็น LoginResponse? (Nullable)
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _context.users
            .Where(u => u.username == request.Username)
            .Select(u => new
            {
                u.id,
                u.username,
                u.password,
                u.role
            })
            .FirstOrDefaultAsync();

        // ✅ ถ้าหาไม่เจอ หรือ รหัสผิด ให้ return null เงียบๆ (ห้าม throw Exception)
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.password))
        {
            return null;
        }

        var token = _jwt.GenerateToken(user.id, user.username, user.role);

        return new LoginResponse
        {
            UserId = user.id,
            Username = user.username,
            Role = user.role,
            Token = token
        };
    }

    
    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        if (await _context.users.AnyAsync(u => u.username == request.Username))
        {
            return false; // ✅ คืนค่า false ถ้าชื่อซ้ำ
        }

        var user = new user
        {
            username = request.Username,
            password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            role = "USER"
        };

        _context.users.Add(user);
        await _context.SaveChangesAsync();

        return true; // ✅ คืนค่า true ถ้าสมัครสำเร็จ
    }
}