using System.ComponentModel.DataAnnotations;

namespace UniManager.Models.DTOs
{
    public sealed class TeacherCreateDto
    {
        [Required, RegularExpression(@".*\S.*"), StringLength(100)]
        public required string firstName {  get; set; }
        [Required, RegularExpression(@".*\S.*"), StringLength(100)]
        public required string lastName { get; set; }

    }
    public sealed class TeacherUpdateDto
    {
        [Required, RegularExpression(@".*\S.*"), StringLength(100)]
        public string? firstName { get; set; }
        [Required, RegularExpression(@".*\S.*"), StringLength(100)]
        public string? lastName { get; set; }
    }
}
