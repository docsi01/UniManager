using UniManager.Models;
using UniManager.Repository;

namespace UniManager.Logic
{
    public sealed class EnrollmentLogic : IEnrollmentLogic
    {
        private readonly IRepository<Enrollment> _enrollRepo;
        private readonly IRepository<Student> _studentRepo;
        private readonly IRepository<Course> _courseRepo;

        public EnrollmentLogic(
            IRepository<Enrollment> enrollRepo,
            IRepository<Student> studentRepo,
            IRepository<Course> courseRepo
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
            _enrollRepo.Create(newEnrollment);
        }

        public void Update(Enrollment updatedEnrollment)
        {
            _enrollRepo.Update(updatedEnrollment);
        }
    }
}
