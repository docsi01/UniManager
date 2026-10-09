using System.ComponentModel.DataAnnotations;

namespace UniManager.Models.DTOs
{
    public sealed class CourseCreateDto
    {
        [Required, RegularExpression(@".*\S.*"), StringLength(200)]
        public string Title {  get; set; }=string.Empty;
        [Range(1, 6)]
        public int Credits {  get; set; }
        [Range(1, int.MaxValue)]
        public int TeacherId {  get; set; }
        [Range(1, int.MaxValue)]
        public int ClassRoomId {  get; set; }
    }

    public sealed class CourseUpdateDto
    {
        [Required, RegularExpression(@".*\S.*"), StringLength(200)]
        public string Title { get; set; }=string.Empty;
        [Range(1, 6)]
        public int Credits { get; set; }

        //Admin use
        [Range(1, int.MaxValue)]
        public int TeacherId { get; set; }
        [Range(1, int.MaxValue)]
        public int ClassRoomId {  get; set; }

    }
}
