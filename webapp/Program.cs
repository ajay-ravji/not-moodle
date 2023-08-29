using System.Collections.Specialized;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
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


app.Run();
