using UniManager.Models;
using UniManager.Models.DTOs;
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

        public async Task CreateAsync(CourseCreateDto entity)
        {
            var newCourse = new Course
            {
                Title = entity.Title,
                Credits = entity.Credits,
                TeacherId = entity.TeacherId,
                ClassRoomId = entity.ClassRoomId,
            };
            await _repo.CreateAsync(newCourse);
        }
        public async Task<IEnumerable<Course>> ReadAllAsync(string includeProperties = "")
        {
            return await _repo.ReadAllAsync(includeProperties);
        }
        public async Task<Course?> ReadAsync(int id)
        {
            return await _repo.ReadAsync(id);
        }
        public async Task UpdateAsync(int id, CourseUpdateDto entity)
        {
            var existingCourse= await _repo.ReadAsync(id);
            if (existingCourse == null) { throw new Exception($"Course with this ID ({id}) was not found!"); }

            existingCourse.Title = entity.Title;
            existingCourse.Credits = entity.Credits;
            existingCourse.TeacherId = entity.TeacherId;
            existingCourse.ClassRoomId = entity.ClassRoomId;

            await _repo.UpdateAsync(existingCourse);
        }
        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
