using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;


[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase {

    [HttpPost("login")]
    public async Task<IResult> login() {
        var body = new StreamReader(HttpContext.Request.Body);
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());

        if (json["username"] == null) return Results.StatusCode(400);
        if (json["password"] == null) return Results.StatusCode(400);

        DatabaseContext dbContext = new DatabaseContext();
        var users = dbContext.Users.Where(x => x.UserName == json["username"]).ToList();
        if (users.Count == 0) return Results.StatusCode(400);

        User user = users[0];
        if (user.Password != json["password"]) return Results.StatusCode(400);

        var claims = new List<Claim>{
            new Claim("id", user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties{}
        );
        return Results.StatusCode(200);
    }

    [HttpPost("logout")]
    public async Task<IResult> logout() {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Results.StatusCode(200);
    }

    [HttpPost("reset")]
    [Authorize(Policy = "lecturer")]
    public async Task<IResult> reset() {
        var body = new StreamReader(HttpContext.Request.Body);
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());
        
        if (json["username"] == null) return Results.StatusCode(400);
        if (json["password"] == null) return Results.StatusCode(400);

        DatabaseContext dbContext = new DatabaseContext();
        var users = dbContext.Users.Where(x => x.UserName == json["username"]).ToList();
        if (users.Count == 0) return Results.StatusCode(400);

        User user = users[0];
        user.Password = json["password"];
        dbContext.SaveChanges();

        return Results.StatusCode(200);
    }
}