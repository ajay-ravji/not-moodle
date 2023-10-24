using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace webapp.Pages;

public class ScheduleModel : PageModel {
    private readonly ILogger<ScheduleModel> _logger;

    public ScheduleModel(ILogger<ScheduleModel> logger) {
        _logger = logger;
    }

    public Schedule Schedule;
    public List<ScheduleSlot> ScheduleSlotList;
    public bool UserHasClaimed = false;

    public void OnGet() {
        ClaimsIdentity identity = (ClaimsIdentity)HttpContext.User.Identity;
        Claim claim = identity.FindFirst(ClaimTypes.Role);
        if (claim == null) return;

        DatabaseContext context = new DatabaseContext(DatabaseContext.ProductionDB);
        int scheduleId;
        if (!Int32.TryParse(HttpContext.Request.Query["schedule"], out scheduleId)) return;

        List<Schedule> schedules = context.Schedules.Where(x => x.ScheduleId == scheduleId).ToList();
        if (schedules.Count == 0) return;


        Schedule = schedules[0];
        ScheduleSlotList = context.ScheduleSlots.Where(x => x.ScheduleId == scheduleId).ToList();

        if (claim.Value != "student") {
            UserHasClaimed = true;
        } else {
            int userId = Int32.Parse(identity.FindFirst("id").Value);
            UserHasClaimed = context.ScheduleSlots.Where(x => x.StudentId == userId && x.ScheduleId == scheduleId).Count() > 0;
        }
    }
}
