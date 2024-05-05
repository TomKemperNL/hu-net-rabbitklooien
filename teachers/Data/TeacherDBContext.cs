using Microsoft.EntityFrameworkCore;
using Teachers.Inno.HU.Domain;

namespace Teachers.Inno.HU;

public class TeacherDBContext : DbContext
{
    public DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=15432;Database=teachers;Username=postgres;Password=1q2w3e!");
    }
}