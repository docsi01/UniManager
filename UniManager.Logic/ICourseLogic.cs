using UniManager.Models;

namespace UniManager.Logic
{
    public interface ICourseLogic
    {

        IEnumerable<Course> ReadAll();
        Task<IEnumerable<Course>> ReadAllAsync();

        Course Read(int id);
        Task ReadAsync(int id);

        void Create(Course course);
        Task CreateAsync(Course course);
        void Update(Course course);
        Task UpdateAsync(Course course);
        void Delete(int id);
        Task DeleteAsync(int id);
    }
}
