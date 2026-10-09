using UniManager.Models;
using UniManager.Models.DTOs;
using UniManager.Repository;

namespace UniManager.Logic
{
    public sealed class TeacherLogic : ITeacherLogic
    {
        private readonly IGenericRepo<Teacher> _repo;
        public TeacherLogic(IGenericRepo<Teacher> repo)
        {
            _repo = repo;
        }

        public async Task<Teacher> CreateAsync(TeacherCreateDto teacher)
        {
            var newTeacher = new Teacher
            {
                firstName = teacher.firstName,
                lastName = teacher.lastName,
            };
            return await _repo.CreateAsync(newTeacher);
        }

        public async Task<IEnumerable<Teacher>> ReadAllAsync(string includeProperties = "")
        {
            return await _repo.ReadAllAsync(includeProperties);
        }
        public async Task<Teacher?> ReadAsync(int id)
        {
            return await _repo.ReadAsync(id);
        }

        public async Task UpdateAsync(int id, TeacherUpdateDto teacher)
        {
            var existingTeacher = await _repo.ReadAsync(id);
            if (existingTeacher == null) { throw new Exception($"Teacher with this ID ({id}) not found!"); }
            await _repo.UpdateAsync(existingTeacher);
        }
        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
