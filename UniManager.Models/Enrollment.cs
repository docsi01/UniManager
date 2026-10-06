using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace UniManager.Models
{
    public sealed class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId {  get; set; }
        public string ?Grade {  get; set; }
        public string? Status {  get; set; }

        [JsonIgnore]
        public Student? Student { get; set; }
        [JsonIgnore]
        public Course? Course { get; set; }
    }
}
