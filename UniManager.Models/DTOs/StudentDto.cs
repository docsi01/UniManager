using System.ComponentModel.DataAnnotations;

namespace UniManager.Models.DTOs
{
    public sealed class StudentCreateDto : IValidatableObject
    {
        [Required, RegularExpression(@".*\S.*"), StringLength(100)]
        public string? firstName { get; set; }
        [Required, RegularExpression(@".*\S.*"), StringLength(100)]
        public string? lastName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EnrollmentDate == default || EnrollmentDate.Date > DateTime.Today)
            {
                yield return new ValidationResult("EnrollmentDate must be a valid date no later than today.", new[] { nameof(EnrollmentDate) });
            }
        }
    }
    public sealed class StudentUpdateDto
    {
        [Required, RegularExpression(@".*\S.*"), StringLength(100)]
        public string firstName { get; set; } = string.Empty;
        [Required, RegularExpression(@".*\S.*"), StringLength(100)]
        public string lastName { get; set; } = string.Empty;
    }
}
