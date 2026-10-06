using System;
using System.Collections.Generic;
using System.Text;

namespace UniManager.Models.DTOs
{
    public sealed class TeacherCreateDto
    {
        public string firstName {  get; set; }=string.Empty;
        public string lastName { get; set; } = string.Empty;

    }
    public sealed class TeacherUpdateDto
    {
        public string firstName { get; set; }=string.Empty;
        public string lastName { get; set; }= string.Empty;
    }
}
