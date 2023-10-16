using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webapp.Pages;

public class ClassModel : PageModel {
    private readonly ILogger<ClassModel> _logger;

    public ClassModel(ILogger<ClassModel> logger) {
        _logger = logger;
    }

    public List<Class> ClassList;
    public List<User> LecturerList;
    public List<Course> CourseList;

    public void OnGet() {
        DatabaseContext context = new DatabaseContext();
        ClassList = context.Classes.ToList();
        LecturerList = context.Users.Where(x => x.Role == "lecturer").ToList();
        CourseList = context.Courses.ToList();
    }
}
