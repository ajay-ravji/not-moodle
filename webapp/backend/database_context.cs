using Microsoft.EntityFrameworkCore;

class DatabaseContext : DbContext {
    public DbSet<Course> Courses { get; set; }
    public DbSet<User> Users { get; set; }

    public DatabaseContext() {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite($"Data Source=" + Environment.CurrentDirectory + "\\database.db");
    }
}

