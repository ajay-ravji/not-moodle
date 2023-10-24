using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;

namespace testing;

[TestClass]
public class UnitTest1 {
    [TestMethod]
    public void TestCourseQueries() {
        DatabaseContext context = new DatabaseContext(DatabaseContext.TestDB);

        // Insertion
        string courseName = "This is a test course";
        context.Add(new Course{ Name = courseName });
        context.SaveChanges();

        Course course = context.Courses.Where(x => x.Name == courseName).First();
        Assert.IsNotNull(course);
        Assert.AreEqual(courseName, course.Name);

        // Deletion
        context.Remove(course);
        context.SaveChanges();

        Assert.AreEqual(0, context.Courses.Where(x => x.Name == courseName).Count());
    }

    [TestMethod]
    public void TestUserQueries() {
        DatabaseContext context = new DatabaseContext(DatabaseContext.TestDB);

        // Insertion
        string role = "lecturer";
        string username = "admin";
        string password = "password";
        string name = "Administrator";
        context.Add(new User{ Role = role, UserName = username, Password = password, Name = name });
        context.SaveChanges();

        User user = context.Users.Where(x => x.UserName == username).First();
        Assert.IsNotNull(user);
        Assert.AreEqual(user.Password, password);
        Assert.AreEqual(user.Name, name);

        // Deletion
        context.Remove(user);
        context.SaveChanges();
        Assert.AreEqual(0, context.Users.Where(x => x.UserName == username).Count());
    }

    [TestMethod]
    public void TestClassQueries() {
        DatabaseContext context = new DatabaseContext(DatabaseContext.TestDB);
        
        // Setup
        context.Add(new Course{ Name = "This is a test course" });
        context.Add(new User{ Role = "lecturer", UserName = "admin", Password = "password", Name = "Administrator" });
        context.SaveChanges();
        Course course = context.Courses.First();
        User lecturer = context.Users.First();

        // Insertion
        string name = "Class name";
        context.Add(new Class{ Name = name, CourseId = course.CourseId, LecturerId = lecturer.UserId });
        context.SaveChanges();

        Class class_ = context.Classes.Where(x => x.Name == name).First();
        Assert.IsNotNull(class_);
        Assert.AreEqual(class_.CourseId, course.CourseId);
        Assert.AreEqual(class_.LecturerId, lecturer.UserId);

        // Deletion
        context.Remove(class_);
        context.SaveChanges();
        Assert.AreEqual(0, context.Classes.Where(x => x.Name == name).Count());

        // Delete setup
        context.Remove(lecturer);
        context.Remove(course);
        context.SaveChanges();
    }

    [TestMethod]
    public void TestScheduleSlotQueries() {
        DatabaseContext context = new DatabaseContext(DatabaseContext.TestDB);

        // Setup
        context.Add(new Course{ Name = "This is a test course" });
        context.Add(new User{ Role = "lecturer", UserName = "admin", Password = "password", Name = "Administrator" });
        context.Add(new User{ Role = "student", UserName = "student", Password = "password", Name = "Student" });
        context.SaveChanges();
        Course course = context.Courses.First();
        User lecturer = context.Users.Where(x => x.Role == "lecturer").First();
        User student = context.Users.Where(x => x.Role == "student").First();

        context.Add(new Class{ Name = "Test class", CourseId = course.CourseId, LecturerId = lecturer.UserId });
        context.SaveChanges();
        Class class_ = context.Classes.First();

        context.Add(new Enrolment{ ClassId = class_.ClassId, StudentId = student.UserId });
        context.Add(new Schedule{ Name = "Test exam", ClassId = class_.ClassId });
        context.SaveChanges();
        Enrolment enrolment = context.Enrolments.First();
        Schedule schedule = context.Schedules.First();

        DateTime time = DateTime.Now;
        context.Add(new ScheduleSlot{ ScheduleId = schedule.ScheduleId, Time = time, StudentId = student.UserId });
        context.SaveChanges();
        ScheduleSlot slot = context.ScheduleSlots.First();
        Assert.IsNotNull(slot);
        Assert.AreEqual(slot.ScheduleId, schedule.ScheduleId);
        Assert.AreEqual(slot.Time, time);
        Assert.AreEqual(slot.StudentId, student.UserId);

        context.Remove(course);
        context.Remove(lecturer);
        context.Remove(student);
        context.SaveChanges();
    }
}