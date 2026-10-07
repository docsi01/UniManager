namespace UniManager.Models.DTOs
{
    public sealed class StudentCreateDto
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
    public sealed class StudentUpdateDto
    {
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
    }
}
