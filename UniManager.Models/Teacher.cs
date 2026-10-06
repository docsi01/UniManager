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
        public string firstName { get; set; }=string.Empty;
        public string lastName { get; set; } = string.Empty;
        public ICollection<Course>? Courses { get; set; }=new List<Course>();
    }
}
