using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


[Route("api/schedule")]
[ApiController]
public class ScheduleController : ControllerBase {

    [HttpPost("create")]
    [Authorize(Policy = "lecturer")]
    public async Task<IResult> create() {
        var body = new StreamReader(HttpContext.Request.Body);
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());

        if (json["name"] == null) return Results.StatusCode(400);

        int classId;
        if (!Int32.TryParse(json["classId"], out classId)) return Results.StatusCode(400);

        DatabaseContext context = new DatabaseContext();
        context.Add(new Schedule{ Name = json["name"], ClassId = classId});
        context.SaveChanges();

        return Results.StatusCode(200);
    }

    [HttpDelete("delete")]
    [Authorize(Policy = "lecturer")]
    public async Task<IResult> delete() {
        var body = new StreamReader(HttpContext.Request.Body);
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());

        if (json["id"] == null) return Results.StatusCode(400);

        int scheduleId;
        if (!Int32.TryParse(json["id"], out scheduleId)) return Results.StatusCode(400);

        DatabaseContext context = new DatabaseContext();
        Schedule schedule = context.Schedules.Where(x => x.ScheduleId == scheduleId).ToList()[0];
        context.Schedules.Remove(schedule);
        context.SaveChanges();

        return Results.StatusCode(200);
    }
}