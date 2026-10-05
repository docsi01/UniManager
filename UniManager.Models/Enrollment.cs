using System;
using System.Collections.Generic;
using System.Text;

namespace UniManager.Models
{
    public sealed class Enrollment
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public int StudentId {  get; set; }

        public string Grade {  get; set; }=string.Empty;

        public Student Student { get; set; } = null!;
        public Course Course { get; set; }=null!;
    }
}
