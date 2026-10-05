using System;
using System.Collections.Generic;
using System.Text;

namespace UniManager.Models
{
    public sealed class Student
    {
        public int Id { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; }=string.Empty;
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; } = new List<Enrollment>();
        
    }
}
