using UniManager.Models;
using UniManager.Models.DTOs;
using UniManager.Repository;

namespace UniManager.Logic
{
    public sealed class CourseLogic : ICourseLogic
    {
        private readonly IGenericRepo<Course> _repo;
        private readonly IGenericRepo<Teacher> _teacherRepo;
        private readonly IGenericRepo<ClassRoom> _classRoomRepo;

        public CourseLogic(
            IGenericRepo<Course> repo,
            IGenericRepo<Teacher> teacherRepo,
            IGenericRepo<ClassRoom> classRoomRepo)
        {
            _repo = repo;
            _teacherRepo = teacherRepo;
            _classRoomRepo = classRoomRepo;
        }

        public async Task<Course> CreateAsync(CourseCreateDto entity)
        {
            await ValidateReferencesAsync(entity.TeacherId, entity.ClassRoomId);
            var newCourse = new Course
            {
                Title = entity.Title,
                Credits = entity.Credits,
                TeacherId = entity.TeacherId,
                ClassRoomId = entity.ClassRoomId,
            };
            return await _repo.CreateAsync(newCourse);
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
            await ValidateReferencesAsync(entity.TeacherId, entity.ClassRoomId);
            var existingCourse= await _repo.ReadAsync(id);
            if (existingCourse == null) { throw new KeyNotFoundException($"Course with this ID ({id}) was not found!"); }

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

        private async Task ValidateReferencesAsync(int teacherId, int classRoomId)
        {
            if (await _teacherRepo.ReadAsync(teacherId) == null)
            {
                throw new KeyNotFoundException($"Teacher with this ID ({teacherId}) was not found!");
            }
            if (await _classRoomRepo.ReadAsync(classRoomId) == null)
            {
                throw new KeyNotFoundException($"Classroom with this ID ({classRoomId}) was not found!");
            }
        }
    }
}
