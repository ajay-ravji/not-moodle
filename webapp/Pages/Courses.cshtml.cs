using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webapp.Pages;

public class CourseModel : PageModel {
    private readonly ILogger<CourseModel> _logger;

    public CourseModel(ILogger<CourseModel> logger) {
        _logger = logger;
    }

    public List<Course> CourseList;

    public void OnGet() {
        DatabaseContext context = new DatabaseContext();
        // context.Add(new Course{ Name = "Tesdasdsadt "});
        // context.SaveChanges();

        CourseList = context.Courses.ToList();
    }
}

