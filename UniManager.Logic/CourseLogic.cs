using UniManager.Models;
using UniManager.Repository;

namespace UniManager.Logic
{
    public sealed class CourseLogic : ICourseLogic
    {
        private readonly IGenericRepo<Course> _repo;
        public CourseLogic(IGenericRepo<Course> repo)
        {
            _repo = repo;
        }
        public IEnumerable<Course> ReadAll()
        {
            return _repo.ReadAll();
        }

        public async Task<IEnumerable<Course>> ReadAllAsync()
        {
            return await _repo.ReadAllAsync();
        }

        public Course Read(int id)
        {
            return _repo.Read(id);
        }

        public Task ReadAsync(int id)
        {
            return _repo.ReadAsync(id);
        }

        public void Create(Course entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _repo.Create(entity);
        }

        public async Task CreateAsync(Course entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _repo.CreateAsync(entity);
        }

        public void Update(Course entity)
        {
            _repo.Update(entity);
        }

        public async Task UpdateAsync(Course entity)
        {
            await _repo.UpdateAsync(entity);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
