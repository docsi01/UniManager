using UniManager.Models;
using UniManager.Models.DTOs;

namespace UniManager.Logic
{
    public interface ICourseLogic
    {
        //async
        Task CreateAsync(CourseCreateDto course);
        Task<IEnumerable<Course>> ReadAllAsync(string includeProperties = "");
        Task<Course?> ReadAsync(int id);
        Task UpdateAsync(int id,CourseUpdateDto course);
        Task DeleteAsync(int id);
    }
}
