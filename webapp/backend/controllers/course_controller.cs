using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


[Route("api/course")]
[ApiController]
public class CourseController : ControllerBase {

    [HttpPost("create")]
    [Authorize(Policy = "lecturer")]
    public async Task<IResult> create() {
        var body = new StreamReader(HttpContext.Request.Body);
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());

        if (json["name"] == null) return Results.StatusCode(400);

        DatabaseContext context = new DatabaseContext();
        context.Add(new Course{ Name = json["name"] });
        context.SaveChanges();

        return Results.StatusCode(200);
    }

    [HttpDelete("delete")]
    [Authorize(Policy = "lecturer")]
    public async Task<IResult> delete() {
        var body = new StreamReader(HttpContext.Request.Body);
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());

        if (json["id"] == null) return Results.StatusCode(400);

        int courseId;
        if (!Int32.TryParse(json["id"], out courseId)) return Results.StatusCode(400);

        DatabaseContext context = new DatabaseContext();
        Course course = context.Courses.Where(x => x.CourseId == courseId).ToList()[0];
        context.Courses.Remove(course);
        context.SaveChanges();

        return Results.StatusCode(200);
    }
}