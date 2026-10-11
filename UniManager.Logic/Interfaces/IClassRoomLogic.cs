using System;
using System.Collections.Generic;
using System.Text;
using UniManager.Models.DTOs;
using UniManager.Models.Models;

namespace UniManager.Logic.Interfaces
{
    public interface IClassRoomLogic
    {
        Task<ClassRoom> CreateAsync(ClassRoomCreateDto dto);
        Task<IEnumerable<ClassRoom>> ReadAllAsync(string includeProperties = "");
        Task<ClassRoom?> ReadAsync(int id);
        Task UpdateAsync(int id, ClassRoomUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
