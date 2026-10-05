using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace UniManager.Models
{
    public sealed class Teacher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A first name is required!")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters!")]
        public string firstName { get; set; }=string.Empty;

        [Required(ErrorMessage = "A last name is required!")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters!")]
        public string lastName { get; set; } = string.Empty;

        public ICollection<Course>? Courses { get; set; }=new List<Course>();
    }
}
