using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace webapp.Pages;

public class ClassesModel : PageModel {
    private readonly ILogger<ClassesModel> _logger;

    public ClassesModel(ILogger<ClassesModel> logger) {
        _logger = logger;
    }

    public List<Class> ClassList;
    public List<bool> Enrolments = new List<bool>();
    public List<User> LecturerList;
    public List<Course> CourseList;

    public void OnGet() {
        DatabaseContext context = new DatabaseContext(DatabaseContext.ProductionDB);
        ClassList = context.Classes.ToList();
        LecturerList = context.Users.Where(x => x.Role == "lecturer").ToList();
        CourseList = context.Courses.ToList();

        ClaimsIdentity identity = (ClaimsIdentity)HttpContext.User.Identity;
        Claim claim = identity.FindFirst(ClaimTypes.Role);
        if (claim == null) return;
        int userId = Int32.Parse(identity.FindFirst("id").Value);

        if (claim.Value == "student") {
            foreach (Class class_ in ClassList) {
                Enrolments.Add(context.Enrolments.Where(x => x.ClassId == class_.ClassId && x.StudentId == userId).Count() > 0);
            }
        }
    }
}
