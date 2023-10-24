using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

[Route("api/slot")]
[ApiController]
public class SlotController : ControllerBase {

    [HttpPost("create")]
    [Authorize(Policy = "lecturer")]
    public async Task<IResult> create() {
        var body = new StreamReader(HttpContext.Request.Body);
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());

        if (json["time"] == null) return Results.StatusCode(400);
        if (json["scheduleId"] == null) return Results.StatusCode(400);

        int scheduleId;
        if (!Int32.TryParse(json["scheduleId"], out scheduleId)) return Results.StatusCode(400);

        DateTime time;
        if (!DateTime.TryParse(json["time"], out time)) return Results.StatusCode(400);

        DatabaseContext context = new DatabaseContext(DatabaseContext.ProductionDB);
        context.Add(new ScheduleSlot{ ScheduleId = scheduleId, Time = time });
        context.SaveChanges();

        return Results.StatusCode(200);
    }

    [HttpDelete("delete")]
    [Authorize(Policy = "lecturer")]
    public async Task<IResult> delete() {
        var body = new StreamReader(HttpContext.Request.Body);
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());

        if (json["id"] == null) return Results.StatusCode(400);

        int scheduleSlotId;
        if (!Int32.TryParse(json["id"], out scheduleSlotId)) return Results.StatusCode(400);

        DatabaseContext context = new DatabaseContext(DatabaseContext.ProductionDB);
        ScheduleSlot scheduleSlot = context.ScheduleSlots.Where(x => x.ScheduleSlotId == scheduleSlotId).ToList()[0];
        context.ScheduleSlots.Remove(scheduleSlot);
        context.SaveChanges();

        return Results.StatusCode(200);
    }

    [HttpPost("claim")]
    public async Task<IResult> claim() {
        var body = new StreamReader(HttpContext.Request.Body);
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());

        if (json["id"] == null) return Results.StatusCode(400);

        int scheduleSlotId;
        if (!Int32.TryParse(json["id"], out scheduleSlotId)) return Results.StatusCode(400);

        DatabaseContext context = new DatabaseContext(DatabaseContext.ProductionDB);
        ScheduleSlot scheduleSlot = context.ScheduleSlots.Where(x => x.ScheduleSlotId == scheduleSlotId).ToList()[0];

        if (scheduleSlot.Student != null) return Results.StatusCode(400);
        ClaimsIdentity identity = (ClaimsIdentity)HttpContext.User.Identity;

        if (identity.FindFirst(ClaimTypes.Role).Value != "student") return Results.StatusCode(400);
        int userId = Int32.Parse(identity.FindFirst("id").Value);

        if (context.ScheduleSlots.Where(x => x.StudentId == userId && x.ScheduleId == scheduleSlot.ScheduleId).Count() > 0) return Results.StatusCode(400);

        scheduleSlot.StudentId = userId;
        context.SaveChanges();

        return Results.StatusCode(200);
    }

    [HttpPost("reset")]
    [Authorize(Policy = "lecturer")]
    public async Task<IResult> reset() {
        var body = new StreamReader(HttpContext.Request.Body);
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(await body.ReadToEndAsync());

        if (json["id"] == null) return Results.StatusCode(400);

        int scheduleSlotId;
        if (!Int32.TryParse(json["id"], out scheduleSlotId)) return Results.StatusCode(400);

        DatabaseContext context = new DatabaseContext(DatabaseContext.ProductionDB);
        ScheduleSlot scheduleSlot = context.ScheduleSlots.Where(x => x.ScheduleSlotId == scheduleSlotId).ToList()[0];
        scheduleSlot.StudentId = null;
        context.SaveChanges();

        return Results.StatusCode(200);
    }
}