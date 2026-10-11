using Microsoft.EntityFrameworkCore;
using UniManager.Models.Models;

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
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.Property(enrollment => enrollment.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();
                entity.Property(enrollment => enrollment.CreatedAt)
                    .HasDefaultValueSql("SYSUTCDATETIME()");
                entity.HasIndex(enrollment => new { enrollment.StudentId, enrollment.CourseId })
                    .IsUnique()
                    .HasFilter("[Status] IN ('Enrolled', 'Waitlisted')");
                entity.HasIndex(enrollment => new { enrollment.CourseId, enrollment.Status, enrollment.CreatedAt });
                entity.HasOne(enrollment => enrollment.Student)
                    .WithMany(student => student.Enrollments)
                    .HasForeignKey(enrollment => enrollment.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(enrollment => enrollment.Course)
                    .WithMany(course => course.Enrollments)
                    .HasForeignKey(enrollment => enrollment.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(course => course.EnrolledCount)
                    .HasDefaultValue(0);
                entity.Property(course => course.RowVersion)
                    .IsRowVersion();
            });
        }
    }
}
