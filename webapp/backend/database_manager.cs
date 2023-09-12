using Microsoft.EntityFrameworkCore;

class DatabaseContext : DbContext {
    public DbSet<Course> Courses { get; set; }
    public DbSet<User> Users { get; set; }
<<<<<<< HEAD

    public DbSet<Lecturer> Lecturer {get; set;}
=======
>>>>>>> bf2176929194f9a740ba2be1bd4ebd1178e6d228

    public DatabaseContext() {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite($"Data Source=" + Environment.CurrentDirectory + "\\database.db");
    }
}

