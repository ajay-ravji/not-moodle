using System.Collections.Specialized;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();


app.MapPost("/api/course/create", (HttpRequest request) => {
    string courseName = request.Form["name"];
    if (courseName == null) return;

    DatabaseContext context = new DatabaseContext();
    context.Add(new Course{ Name = courseName });
    context.SaveChanges();
});

app.MapGet("/api/course/all", (HttpRequest request) => {
    DatabaseContext context = new DatabaseContext();
    return context.Courses;
});

app.MapDelete("/api/course/delete", (HttpRequest request) => {
    string courseIdStr = request.Query["id"];
    if (courseIdStr == null) return;

    int courseId;
    if (!Int32.TryParse(courseIdStr, out courseId)) return;

    DatabaseContext context = new DatabaseContext();
    Course course = context.Courses.Where(x => x.CourseId == courseId).ToList()[0];
    context.Courses.Remove(course);
    context.SaveChanges();
});

app.MapPost("/api/auth/login", async (HttpContext context, HttpRequest request) => {
    var body = new StreamReader(request.Body);
    var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());

    DatabaseContext dbContext = new DatabaseContext();
    var users = dbContext.Users.Where(x => x.UserName == json["username"]).ToList();
    if (users.Count == 0) return Results.StatusCode(400);

    User user = users[0];
    if (user.Password != json["password"]) return Results.StatusCode(400);

    var claims = new List<Claim>{
        new Claim("id", user.UserId.ToString()),
        new Claim(ClaimTypes.Name, "test name")
    };

    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    await context.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        new ClaimsPrincipal(claimsIdentity),
        new AuthenticationProperties{}
    );
    return Results.StatusCode(200);
});

app.MapPost("/api/auth/logout", async (HttpContext context, HttpRequest request) => {
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.StatusCode(200);
});


app.Run();
