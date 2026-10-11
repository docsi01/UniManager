namespace UniManager.Models.Models
{
    public sealed class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public string? Grade { get; set; }
        public EnrollmentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Student? Student { get; set; }
        public Course? Course { get; set; }
    }
}
