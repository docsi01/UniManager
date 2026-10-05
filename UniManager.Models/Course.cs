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

        [Required(ErrorMessage ="The Course title is required!")]
        [MaxLength(100,ErrorMessage ="Title cannot exceed 100 characters!")]
        public string Title { get; set; } = string.Empty;
        [Range(0,10,ErrorMessage ="Credits must be between 0 and 10!")]
        public int Credits { get; set; }

        public int TeacherId {  get; set; }
        [JsonIgnore]
        public Teacher? Teacher { get; set; }

        public int ClassRoomId {  get; set; }
        [JsonIgnore]
        public ClassRoom? ClassRoom { get; set; }

        [JsonIgnore]
        public ICollection<Enrollment>? Enrollments {  get; set; } = new List<Enrollment>();
    }
}
