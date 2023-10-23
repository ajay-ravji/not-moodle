using System.Collections.Specialized;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Web;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.Events.OnRedirectToAccessDenied = options.Events.OnRedirectToLogin = context => {
            context.Response.StatusCode = (int)(HttpStatusCode.Unauthorized);
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization(options => {
    options.AddPolicy("lecturer", policy => policy.RequireRole("lecturer"));
});

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
app.MapControllers();


// app.MapPost("/api/course/create", async (HttpRequest request) => {
    
// }).RequireAuthorization("lecturer");

// app.MapDelete("/api/course/delete", async (HttpRequest request) => {
    
// }).RequireAuthorization("lecturer");


// app.MapPost("/api/class/create", async (HttpRequest request) => {
    
// }).RequireAuthorization("lecturer");

// app.MapDelete("/api/class/delete", async (HttpRequest request) => {
    
// }).RequireAuthorization("lecturer");


// app.MapPost("/api/auth/login", async (HttpContext context, HttpRequest request) => {
    
// });

// app.MapPost("/api/auth/logout", async (HttpContext context, HttpRequest request) => {
    
// });

// app.MapPost("/api/auth/reset", async (HttpRequest request) => {
    
// });

app.Run();
