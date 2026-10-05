using System;
using System.Collections.Generic;
using System.Text;

namespace UniManager.Models
{
    public sealed class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Enrollment> Enrollments{ get; set; }
        
    }
}
