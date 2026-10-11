using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace UniManager.Models.Models
{
    public sealed class Course
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int Credits { get; set; }
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public int ClassRoomId { get; set; }
        public ClassRoom? ClassRoom { get; set; }
        public int EnrolledCount { get; set; }
        public byte[] RowVersion { get; set; } = null!;
        public ICollection<Enrollment>? Enrollments { get; set; } = new List<Enrollment>();
    }
}
