using MeetingRoomBooking.Client.Auth;
using MeetingRoomBooking.Client.Components;
using MeetingRoomBooking.Client.Providers; // ✅ เพิ่ม
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Authorization
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<BookingProvider>();
builder.Services.AddScoped<RoomProvider>();
// JWT auth state
builder.Services.AddScoped<JwtAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<JwtAuthStateProvider>()
);
builder.Services.AddScoped<AdminRoomProvider>();

// ✅ Register AuthProvider (ตัวนี้แหละที่ขาด)
builder.Services.AddScoped<AuthProvider>();


builder.Services.AddScoped(sp =>
{
    var handler = new HttpClientHandler();
    
    handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

  
    return new HttpClient(handler)
    {
  
        BaseAddress = new Uri("https://127.0.0.1:5001/")
    };
});


var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
