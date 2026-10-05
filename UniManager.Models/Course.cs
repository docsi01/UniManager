using System;
using System.Collections.Generic;
using System.Text;

namespace UniManager.Models
{
    public sealed class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }

        public int TeacherId {  get; set; }
        public Teacher Teacher { get; set; } = null!;

        public int ClassRoomId {  get; set; }
        public ClassRoom ClassRoom { get; set; }=null!;

        public ICollection<Enrollment> Enrollments {  get; set; }
    }
}
