using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace UniManager.Models
{
    public sealed class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int TeacherId {  get; set; }
        public Teacher? Teacher { get; set; }
        public int ClassRoomId {  get; set; }
        public ClassRoom? ClassRoom { get; set; }
        public ICollection<Enrollment>? Enrollments {  get; set; } = new List<Enrollment>();
    }
}
