using System;
using System.Collections.Generic;
using System.Text;

namespace UniManager.Models.DTOs
{
    public sealed class TeacherCreateDto
    {
        public required string firstName {  get; set; }
        public required string lastName { get; set; }

    }
    public sealed class TeacherUpdateDto
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
    }
}
