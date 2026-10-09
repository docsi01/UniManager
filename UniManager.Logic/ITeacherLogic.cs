using UniManager.Models;
using UniManager.Models.DTOs;

namespace UniManager.Logic
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
