using UniManager.Repository;
using UniManager.Models.DTOs;
using UniManager.Logic.Interfaces;
using UniManager.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace UniManager.Logic.Logic
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

        public async Task<Enrollment> EnrollAsync(EnrollmentCreateDto newEnrollment)
        {
            for (var attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    var student = await _studentRepo.ReadAsync(newEnrollment.StudentId);
                    if (student == null)
                    {
                        throw new KeyNotFoundException($"Student with this ID does not exist: {newEnrollment.StudentId}");
                    }

                    var course = await _courseRepo.ReadAsync(newEnrollment.CourseId, "ClassRoom");
                    if (course == null)
                    {
                        throw new KeyNotFoundException($"Course with this ID does not exist: {newEnrollment.CourseId}");
                    }

                    if (course.ClassRoom == null)
                    {
                        throw new KeyNotFoundException($"Classroom with this ID does not exist: {course.ClassRoomId}");
                    }

                    var allEnrollments = await _enrollRepo.ReadAllAsync();
                    if (allEnrollments.Any(enrollment =>
                        enrollment.StudentId == newEnrollment.StudentId &&
                        enrollment.CourseId == newEnrollment.CourseId &&
                        enrollment.Status is EnrollmentStatus.Enrolled or EnrollmentStatus.Waitlisted))
                    {
                        throw new InvalidOperationException("This student is already enrolled in this course!");
                    }

                    var enrollment = new Enrollment
                    {
                        StudentId = newEnrollment.StudentId,
                        CourseId = newEnrollment.CourseId,
                        CreatedAt = DateTime.UtcNow,
                        Grade = null,
                        Status = course.EnrolledCount < course.ClassRoom.Capacity
                            ? EnrollmentStatus.Enrolled
                            : EnrollmentStatus.Waitlisted
                    };

                    if (enrollment.Status == EnrollmentStatus.Enrolled)
                    {
                        course.EnrolledCount++;
                        _courseRepo.UpdateWithoutSaving(course);
                    }

                    _enrollRepo.AddWithoutSaving(enrollment);
                    await _enrollRepo.SaveChangesAsync();
                    return enrollment;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _enrollRepo.ClearChangeTracker();
                    if (attempt == 3)
                    {
                        throw new InvalidOperationException("Could not enroll the student after three concurrent updates.", ex);
                    }
                }
            }

            throw new InvalidOperationException("Could not enroll the student after three concurrent updates.");
        }
        public async Task<IEnumerable<Enrollment>> ReadAllAsync(string includeProperties = "")
        {
            return await _enrollRepo.ReadAllAsync(includeProperties);
        }
        public async Task<Enrollment> ReadAsync(int id)
        {
            var enrollment = await _enrollRepo.ReadAsync(id);
            if (enrollment == null)
            {
                throw new KeyNotFoundException($"Enrollment with this ID ({id}) not found!");
            }
            return enrollment;
        }
        public async Task UpdateAsync(int id, EnrollmentUpdateDto updatedEnrollment)
        {
            var existingEnrollment = await _enrollRepo.ReadAsync(id);
            if (existingEnrollment == null)
            {
                throw new KeyNotFoundException($"Enrollment with this ID ({id}) not found!");
            }

            existingEnrollment.Grade = updatedEnrollment.Grade;
            _enrollRepo.UpdateWithoutSaving(existingEnrollment);
            await _enrollRepo.SaveChangesAsync();
        }

        public async Task DropAsync(int id)
        {
            for (var attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    var enrollment = await _enrollRepo.ReadAsync(id);
                    if (enrollment == null)
                    {
                        throw new KeyNotFoundException($"Enrollment with this ID ({id}) not found!");
                    }
                    if (enrollment.Status == EnrollmentStatus.Dropped)
                    {
                        return;
                    }

                    if (enrollment.Status == EnrollmentStatus.Enrolled)
                    {
                        var course = await _courseRepo.ReadAsync(enrollment.CourseId, "ClassRoom");
                        if (course == null)
                        {
                            throw new KeyNotFoundException($"Course with this ID ({enrollment.CourseId}) not found!");
                        }

                        course.EnrolledCount = Math.Max(0, course.EnrolledCount - 1);
                        enrollment.Status = EnrollmentStatus.Dropped;
                        _courseRepo.UpdateWithoutSaving(course);

                        var nextEnrollment = (await _enrollRepo.ReadAllAsync())
                            .Where(waitlisted => waitlisted.CourseId == course.Id && waitlisted.Status == EnrollmentStatus.Waitlisted)
                            .OrderBy(waitlisted => waitlisted.CreatedAt)
                            .ThenBy(waitlisted => waitlisted.Id)
                            .FirstOrDefault();

                        if (nextEnrollment != null)
                        {
                            nextEnrollment.Status = EnrollmentStatus.Enrolled;
                            course.EnrolledCount++;
                            _courseRepo.UpdateWithoutSaving(course);
                            _enrollRepo.UpdateWithoutSaving(nextEnrollment);
                        }
                    }
                    else
                    {
                        enrollment.Status = EnrollmentStatus.Dropped;
                    }

                    _enrollRepo.UpdateWithoutSaving(enrollment);
                    await _enrollRepo.SaveChangesAsync();
                    return;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _enrollRepo.ClearChangeTracker();
                    if (attempt == 3)
                    {
                        throw new InvalidOperationException("Could not drop the enrollment after three concurrent updates.", ex);
                    }
                }
            }

            throw new InvalidOperationException("Could not drop the enrollment after three concurrent updates.");
        }
    }
}
