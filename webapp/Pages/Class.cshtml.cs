using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webapp.Pages;

public class ClassModel : PageModel {
    private readonly ILogger<ClassModel> _logger;

    public ClassModel(ILogger<ClassModel> logger) {
        _logger = logger;
    }

    public Class Class;
    public List<Schedule> ScheduleList;
    public List<User> StudentList;

    public void OnGet() {
        DatabaseContext context = new DatabaseContext();
        int classId;
        if (!Int32.TryParse(HttpContext.Request.Query["class"], out classId)) return;

        List<Class> classes = context.Classes.Where(x => x.ClassId == classId).ToList();
        if (classes.Count == 0) return;

        Class = classes[0];
        ScheduleList = context.Schedules.Where(x => x.ClassId == classId).ToList();
        StudentList = context.Enrolments.Where(x => x.ClassId == classId).Select(x => x.Student).ToList();
    }
}
