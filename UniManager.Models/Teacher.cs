using System;
using System.Collections.Generic;
using System.Text;

namespace UniManager.Models
{
    public sealed class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;

        public ICollection<Course> Courses { get; set; }=new List<Course>();
    }
}
