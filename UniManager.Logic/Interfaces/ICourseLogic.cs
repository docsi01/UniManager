using UniManager.Models.DTOs;
using UniManager.Models.Models;

namespace UniManager.Logic.Interfaces
{
    public interface ICourseLogic
    {
        //async
        Task<Course> CreateAsync(CourseCreateDto course);
        Task<IEnumerable<Course>> ReadAllAsync(string includeProperties = "");
        Task<Course?> ReadAsync(int id);
        Task UpdateAsync(int id,CourseUpdateDto course);
        Task DeleteAsync(int id);
    }
}
