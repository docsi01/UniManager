using UniManager.Models.DTOs;
using UniManager.Models.Models;

namespace UniManager.Logic.Interfaces
{
    public interface ITeacherLogic
    {
        Task<Teacher> CreateAsync(TeacherCreateDto teacher);
        Task<IEnumerable<Teacher>> ReadAllAsync(string includeProperties = "");
        Task<Teacher?> ReadAsync(int id);
        Task UpdateAsync(int id, TeacherUpdateDto teacher);
        Task DeleteAsync(int id);
    }
}
