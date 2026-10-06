using System;
using System.Collections.Generic;
using System.Text;

namespace UniManager.Models.DTOs
{
    public sealed class EnrollmentCreateDto
    {
        public int StudentId {  get; set; }
        public int CourseId {  get; set; }
        public string? Grade {  get; set; }
        public string? Status {  get; set; }
    }
    public sealed class EnrollmentUpdateDto
    {
        public string? Grade {  get; set; }
        public string? Status {  get; set; }
    }
}
