using UniManager.Models;
using UniManager.Repository;
using UniManager.Models.DTOs;

namespace UniManager.Logic
{
    public sealed class EnrollmentLogic : IEnrollmentLogic
    {
        private readonly IGenericRepo<Enrollment> _enrollRepo;
        private readonly IGenericRepo<Student> _studentRepo;
        private readonly IGenericRepo<Course> _courseRepo;

        public EnrollmentLogic(
            IGenericRepo<Enrollment> enrollRepo,
            IGenericRepo<Student> studentRepo,
            IGenericRepo<Course> courseRepo
            )
        {
            _enrollRepo = enrollRepo;
            _studentRepo = studentRepo;
            _courseRepo = courseRepo;
        }

        public async Task CreateAsync(EnrollmentCreateDto newEnrollment)
        {
            var student = await _studentRepo.ReadAsync(newEnrollment.StudentId);
            if (student == null)
            {
                throw new Exception($"Student with this ID does not exist: {newEnrollment.StudentId}");
            }

            var course = await _courseRepo.ReadAsync(newEnrollment.CourseId);
            if (course == null)
            {
                throw new Exception($"Course with this ID does not exist: {newEnrollment.CourseId}");
            }

            var allEnrollments = await _enrollRepo.ReadAllAsync();
            bool isAlreadyEnrolled = allEnrollments.Any(e =>
                e.StudentId == newEnrollment.StudentId &&
                e.CourseId == newEnrollment.CourseId);
            if (isAlreadyEnrolled)
            {
                throw new Exception("This student is already enrolled in this course!");
            }

            var enrollment = new Enrollment
            {
                StudentId = newEnrollment.StudentId,
                CourseId = newEnrollment.CourseId,
                Grade = "Not Graded",
                Status = "Enrolled"
            };
            await _enrollRepo.CreateAsync(enrollment);
        }
        public async Task<IEnumerable<Enrollment>> ReadAllAsync(string includeProperties = "")
        {
            return await _enrollRepo.ReadAllAsync(includeProperties);
        }
        public async Task<Enrollment> ReadAsync(int id)
        {
            return await _enrollRepo.ReadAsync(id);
        }
        public async Task UpdateAsync(int id,EnrollmentUpdateDto updatedEnrollment)
        {
            var existingEnrollment = await _enrollRepo.ReadAsync(id);
            if (existingEnrollment == null) { throw new Exception($"Student with this ID ({id}) not found!"); }
            
            existingEnrollment.Grade = updatedEnrollment.Grade;
            existingEnrollment.Status = updatedEnrollment.Status;

            await _enrollRepo.UpdateAsync(existingEnrollment);
        }
        public async Task DeleteAsync(int id)
        {
            await _enrollRepo.DeleteAsync(id);
        }
    }
}
