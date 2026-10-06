using System;
using System.Collections.Generic;
using System.Text;
using UniManager.Models;
using UniManager.Models.DTOs;

namespace UniManager.Logic
{
    public interface IClassRoomLogic
    {
        Task CreateAsync(ClassRoomCreateDto dto);
        Task<IEnumerable<ClassRoom>> ReadAllAsync(string includeProperties = "");
        Task<ClassRoom?> ReadAsync(int id);
        Task UpdateAsync(int id, ClassRoomUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
