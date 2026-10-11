using System.ComponentModel.DataAnnotations;

namespace UniManager.Models.DTOs
{
    public sealed class EnrollmentCreateDto
    {
        [Range(1, int.MaxValue)]
        public int StudentId {  get; set; }
        [Range(1, int.MaxValue)]
        public int CourseId {  get; set; }
        [Required, RegularExpression(@".*\S.*"), StringLength(50)]
        public string? Grade {  get; set; }
        [Required, RegularExpression(@".*\S.*"), StringLength(50)]
        public string? Status {  get; set; }
    }
    public sealed class EnrollmentUpdateDto
    {
        [Required, RegularExpression(@".*\S.*"), StringLength(50)]
        public string? Grade {  get; set; }
        [Required, RegularExpression(@".*\S.*"), StringLength(50)]
        public string? Status {  get; set; }
    }
}
