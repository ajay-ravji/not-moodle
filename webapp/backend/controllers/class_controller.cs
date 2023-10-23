using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


[Route("api/class")]
[ApiController]
public class ClassController : ControllerBase {

    [HttpPost("create")]
    [Authorize(Policy = "lecturer")]
    public async Task<IResult> create() {
        var body = new StreamReader(HttpContext.Request.Body);
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());

        if (json["name"] == null) return Results.StatusCode(400);
        if (json["courseId"] == null) return Results.StatusCode(400);
        if (json["lecturerId"] == null) return Results.StatusCode(400);

        int courseId;
        if (!Int32.TryParse(json["courseId"], out courseId)) return Results.StatusCode(400);
        int lecturerId;
        if (!Int32.TryParse(json["lecturerId"], out lecturerId)) return Results.StatusCode(400);

        DatabaseContext context = new DatabaseContext();
        context.Add(new Class{ Name = json["name"], CourseId = courseId, LecturerId = lecturerId });
        context.SaveChanges();

        return Results.StatusCode(200);
    }

    [HttpDelete("delete")]
    [Authorize(Policy = "lecturer")]
    public async Task<IResult> delete() {
        var body = new StreamReader(HttpContext.Request.Body);
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());

        if (json["id"] == null) return Results.StatusCode(400);

        int classId;
        if (!Int32.TryParse(json["id"], out classId)) return Results.StatusCode(400);

        DatabaseContext context = new DatabaseContext();
        Class class_ = context.Classes.Where(x => x.ClassId == classId).ToList()[0];
        context.Classes.Remove(class_);
        context.SaveChanges();

        return Results.StatusCode(200);
    }
}