using Microsoft.EntityFrameworkCore;

class DatabaseContext : DbContext {
    public DbSet<Course> Courses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<ScheduleSlot> ScheduleSlots { get; set; }
    public DbSet<Enrolment> Enrolments { get; set; }

    public DatabaseContext() {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite($"Data Source=" + Environment.CurrentDirectory + "\\database.db");
    }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<Enrolment>().HasKey(x => new { x.ClassId, x.StudentId });
    }
}

