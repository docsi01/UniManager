using System;
using System.Collections.Generic;
using System.Text;

namespace UniManager.Models.DTOs
{
    public sealed class ClassRoomCreateDto
    {
        public string RoomName {  get; set; }=string.Empty;
        public int Capacity {  get; set; }
    }
    public sealed class ClassRoomUpdateDto
    {
        public string RoomName { get; set; }=string.Empty;
        public int Capacity { get; set; }
    }
}
