using System.ComponentModel.DataAnnotations;

namespace UniManager.Models.DTOs
{
    public sealed class ClassRoomCreateDto
    {
        [Required, RegularExpression(@".*\S.*"), StringLength(100)]
        public string RoomName {  get; set; }=string.Empty;
        [Range(1, int.MaxValue)]
        public int Capacity {  get; set; }
    }
    public sealed class ClassRoomUpdateDto
    {
        [Required, RegularExpression(@".*\S.*"), StringLength(100)]
        public string RoomName { get; set; }=string.Empty;
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }
    }
}
