using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UniManager.Models
{
    public sealed class ClassRoom
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public ICollection<Course>? Courses { get; set; } = new List<Course>();

    }
}
