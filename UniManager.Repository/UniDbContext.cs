using Microsoft.EntityFrameworkCore;
using UniManager.Models;

namespace UniManager.Repository
{
    public sealed class UniDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }

        public UniDbContext(DbContextOptions<UniDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
