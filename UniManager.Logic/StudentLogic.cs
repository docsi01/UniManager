using UniManager.Models;
using UniManager.Models.DTOs;
using UniManager.Repository;

namespace UniManager.Logic
{
    public sealed class StudentLogic : IStudentLogic
    {
        private readonly IGenericRepo<Student> _repo;
        public StudentLogic(IGenericRepo<Student> repo)
        {
            _repo = repo;
        }

        public async Task<Student> CreateAsync(StudentCreateDto student)
        {
            var newStudent = new Student
            {
                firstName = student.firstName,
                lastName = student.lastName,
                EnrollmentDate = student.EnrollmentDate,
            };
            return await _repo.CreateAsync(newStudent);
            
        }

        public async Task<IEnumerable<Student>> ReadAllAsync(string includeProperties = "")
        {
            return await _repo.ReadAllAsync(includeProperties);
        }

        public async Task<Student> ReadAsync(int id)
        {
            return await _repo.ReadAsync(id);
        }

        public async Task UpdateAsync(int id, StudentUpdateDto student)
        {
            var existingStudent = await _repo.ReadAsync(id);
            if (existingStudent == null) { throw new Exception($"Student with this ID ({id}) not found!"); }

            existingStudent.firstName = student.firstName;
            existingStudent.lastName = student.lastName;

            await _repo.UpdateAsync(existingStudent);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
