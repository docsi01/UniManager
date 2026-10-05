using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UniManager.Models
{
    public sealed class ClassRoom
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The classrooms name is required!")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters!")]
        public string RoomName { get; set; } = string.Empty;
        public int Capacity { get; set; }

        [JsonIgnore]
        public ICollection<Course>? Courses { get; set; } = new List<Course>();

    }
}
