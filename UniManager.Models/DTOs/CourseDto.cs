using System;
using System.Collections.Generic;
using System.Text;

namespace UniManager.Models.DTOs
{
    public sealed class CourseCreateDto
    {
        public string Title {  get; set; }=string.Empty;
        public int Credits {  get; set; }
        public int TeacherId {  get; set; }
        public int ClassRoomId {  get; set; }
    }

    public sealed class CourseUpdateDto
    {
        public string Title { get; set; }=string.Empty;
        public int Credits { get; set; }

        //Admin use
        public int TeacherId { get; set; }
        public int ClassRoomId {  get; set; }

    }
}
