using UniManager.Models;
using UniManager.Repository;

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

        public IEnumerable<Enrollment> ReadAll()
        {
            return _enrollRepo.ReadAll();
        }

        public void Create(Enrollment newEnrollment)
        {
            var student = _studentRepo.Read(newEnrollment.StudentId);
            if (student == null)
            {
                throw new Exception($"Student with this ID does not exist: {newEnrollment.StudentId}");
            }

            var course = _courseRepo.Read(newEnrollment.CourseId);
            if (course == null)
            {
                throw new Exception($"Course with this ID does not exist: {newEnrollment.CourseId}");
            }

            var allEnrollments = _enrollRepo.ReadAll();
            bool isAlreadyEnrolled = allEnrollments.Any(e =>
                e.StudentId == newEnrollment.StudentId &&
                e.CourseId == newEnrollment.CourseId);
            if (isAlreadyEnrolled)
            {
                throw new Exception("This student is already enrolled in this course!");
            }

            newEnrollment.Grade = "Not Graded";
            newEnrollment.Status = "Enrolled";
            _enrollRepo.Create(newEnrollment);
        }

        public void Update(Enrollment updatedEnrollment)
        {
            _enrollRepo.Update(updatedEnrollment);
        }

        public async Task<IEnumerable<Enrollment>> ReadAllAsync()
        {
            return await _enrollRepo.ReadAllAsync();
        }

        public async Task CreateAsync(Enrollment newEnrollment)
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

            newEnrollment.Grade = "Not Graded";
            await _enrollRepo.CreateAsync(newEnrollment);
        }

        public async Task UpdateAsync(Enrollment updatedEnrollment)
        {
            await _enrollRepo.UpdateAsync(updatedEnrollment);
        }
    }
}
