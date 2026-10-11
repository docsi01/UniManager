using Moq;
using UniManager.Logic.Logic;
using UniManager.Models.DTOs;
using UniManager.Models.Models;
using UniManager.Repository;

namespace UniManager.Tests
{
    [TestFixture]
    public class EnrollmentLogicTests
    {
        [Test]
        public async Task EnrollAsync_WhenCourseHasFreeSeat_EnrollsAndIncrementsCount()
        {
            var enrollmentRepo = new Mock<IGenericRepo<Enrollment>>();
            var studentRepo = new Mock<IGenericRepo<Student>>();
            var courseRepo = new Mock<IGenericRepo<Course>>();
            var course = new Course { Id = 4, ClassRoom = new ClassRoom { Capacity = 1 } };
            studentRepo.Setup(repo => repo.ReadAsync(3)).ReturnsAsync(new Student { Id = 3 });
            courseRepo.Setup(repo => repo.ReadAsync(4, "ClassRoom")).ReturnsAsync(course);
            enrollmentRepo.Setup(repo => repo.ReadAllAsync(It.IsAny<string>())).ReturnsAsync(Array.Empty<Enrollment>());
            enrollmentRepo.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            var logic = new EnrollmentLogic(enrollmentRepo.Object, studentRepo.Object, courseRepo.Object);
            var result = await logic.EnrollAsync(new EnrollmentCreateDto { StudentId = 3, CourseId = 4 });

            Assert.That(result.Status, Is.EqualTo(EnrollmentStatus.Enrolled));
            Assert.That(result.Grade, Is.Null);
            Assert.That(result.CreatedAt.Kind, Is.EqualTo(DateTimeKind.Utc));
            Assert.That(course.EnrolledCount, Is.EqualTo(1));
            courseRepo.Verify(repo => repo.UpdateWithoutSaving(course), Times.Once);
            enrollmentRepo.Verify(repo => repo.AddWithoutSaving(result), Times.Once);
            enrollmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void EnrollAsync_WhenStudentAlreadyHasActiveEnrollment_ThrowsConflictException()
        {
            var enrollmentRepo = new Mock<IGenericRepo<Enrollment>>();
            var studentRepo = new Mock<IGenericRepo<Student>>();
            var courseRepo = new Mock<IGenericRepo<Course>>();
            studentRepo.Setup(repo => repo.ReadAsync(3)).ReturnsAsync(new Student { Id = 3 });
            courseRepo.Setup(repo => repo.ReadAsync(4, "ClassRoom"))
                .ReturnsAsync(new Course { Id = 4, ClassRoom = new ClassRoom { Capacity = 5 } });
            enrollmentRepo.Setup(repo => repo.ReadAllAsync(It.IsAny<string>())).ReturnsAsync(new[]
            {
                new Enrollment { StudentId = 3, CourseId = 4, Status = EnrollmentStatus.Enrolled }
            });

            var logic = new EnrollmentLogic(enrollmentRepo.Object, studentRepo.Object, courseRepo.Object);

            Assert.ThrowsAsync<InvalidOperationException>(() => logic.EnrollAsync(new EnrollmentCreateDto
            {
                StudentId = 3,
                CourseId = 4
            }));
            enrollmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Never);
            enrollmentRepo.Verify(repo => repo.AddWithoutSaving(It.IsAny<Enrollment>()), Times.Never);
        }

        [Test]
        public void EnrollAsync_WhenStudentDoesNotExist_ThrowsNotFoundException()
        {
            var enrollmentRepo = new Mock<IGenericRepo<Enrollment>>();
            var studentRepo = new Mock<IGenericRepo<Student>>();
            var courseRepo = new Mock<IGenericRepo<Course>>();
            studentRepo.Setup(repo => repo.ReadAsync(3)).ReturnsAsync((Student?)null);
            var logic = new EnrollmentLogic(enrollmentRepo.Object, studentRepo.Object, courseRepo.Object);

            Assert.ThrowsAsync<KeyNotFoundException>(() => logic.EnrollAsync(new EnrollmentCreateDto
            {
                StudentId = 3,
                CourseId = 4
            }));
        }

        [Test]
        public void EnrollAsync_WhenCourseDoesNotExist_ThrowsNotFoundException()
        {
            var enrollmentRepo = new Mock<IGenericRepo<Enrollment>>();
            var studentRepo = new Mock<IGenericRepo<Student>>();
            var courseRepo = new Mock<IGenericRepo<Course>>();
            studentRepo.Setup(repo => repo.ReadAsync(3)).ReturnsAsync(new Student { Id = 3 });
            courseRepo.Setup(repo => repo.ReadAsync(4, "ClassRoom")).ReturnsAsync((Course?)null);
            var logic = new EnrollmentLogic(enrollmentRepo.Object, studentRepo.Object, courseRepo.Object);

            Assert.ThrowsAsync<KeyNotFoundException>(() => logic.EnrollAsync(new EnrollmentCreateDto
            {
                StudentId = 3,
                CourseId = 4
            }));
        }

        [Test]
        public async Task DropAsync_WhenEnrollmentWasAlreadyDropped_IsNoOp()
        {
            var enrollmentRepo = new Mock<IGenericRepo<Enrollment>>();
            var studentRepo = new Mock<IGenericRepo<Student>>();
            var courseRepo = new Mock<IGenericRepo<Course>>();
            enrollmentRepo.Setup(repo => repo.ReadAsync(22)).ReturnsAsync(new Enrollment
            {
                Id = 22,
                Status = EnrollmentStatus.Dropped
            });

            var logic = new EnrollmentLogic(enrollmentRepo.Object, studentRepo.Object, courseRepo.Object);
            await logic.DropAsync(22);

            enrollmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Never);
            courseRepo.Verify(repo => repo.UpdateWithoutSaving(It.IsAny<Course>()), Times.Never);
        }

        [Test]
        public async Task DropAsync_WhenWaitlistedStudentDrops_DoesNotChangeCourseCountOrPromote()
        {
            var enrollmentRepo = new Mock<IGenericRepo<Enrollment>>();
            var studentRepo = new Mock<IGenericRepo<Student>>();
            var courseRepo = new Mock<IGenericRepo<Course>>();
            var waitlistedEnrollment = new Enrollment { Id = 21, CourseId = 4, Status = EnrollmentStatus.Waitlisted };
            enrollmentRepo.Setup(repo => repo.ReadAsync(21)).ReturnsAsync(waitlistedEnrollment);
            enrollmentRepo.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            var logic = new EnrollmentLogic(enrollmentRepo.Object, studentRepo.Object, courseRepo.Object);
            await logic.DropAsync(21);

            Assert.That(waitlistedEnrollment.Status, Is.EqualTo(EnrollmentStatus.Dropped));
            courseRepo.Verify(repo => repo.ReadAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            courseRepo.Verify(repo => repo.UpdateWithoutSaving(It.IsAny<Course>()), Times.Never);
            enrollmentRepo.Verify(repo => repo.ReadAllAsync(It.IsAny<string>()), Times.Never);
            enrollmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task EnrollAsync_WhenCourseRowVersionConflicts_RetriesAndWaitlistsIfNowFull()
        {
            var enrollmentRepo = new Mock<IGenericRepo<Enrollment>>();
            var studentRepo = new Mock<IGenericRepo<Student>>();
            var courseRepo = new Mock<IGenericRepo<Course>>();
            var firstCourse = new Course { Id = 4, ClassRoom = new ClassRoom { Capacity = 1 } };
            var reloadedCourse = new Course { Id = 4, ClassRoom = new ClassRoom { Capacity = 1 }, EnrolledCount = 1 };
            studentRepo.Setup(repo => repo.ReadAsync(3)).ReturnsAsync(new Student { Id = 3 });
            courseRepo.SetupSequence(repo => repo.ReadAsync(4, "ClassRoom"))
                .ReturnsAsync(firstCourse)
                .ReturnsAsync(reloadedCourse);
            enrollmentRepo.Setup(repo => repo.ReadAllAsync(It.IsAny<string>())).ReturnsAsync(Array.Empty<Enrollment>());
            var saveAttempts = 0;
            enrollmentRepo.Setup(repo => repo.SaveChangesAsync()).Returns(() =>
            {
                saveAttempts++;
                return saveAttempts == 1
                    ? Task.FromException(new Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException())
                    : Task.CompletedTask;
            });

            var logic = new EnrollmentLogic(enrollmentRepo.Object, studentRepo.Object, courseRepo.Object);
            var result = await logic.EnrollAsync(new EnrollmentCreateDto { StudentId = 3, CourseId = 4 });

            Assert.That(result.Status, Is.EqualTo(EnrollmentStatus.Waitlisted));
            Assert.That(saveAttempts, Is.EqualTo(2));
            enrollmentRepo.Verify(repo => repo.ClearChangeTracker(), Times.Once);
        }

        [Test]
        public async Task EnrollAsync_WhenCourseIsFull_AddsEnrollmentToWaitlist()
        {
            var enrollmentRepo = new Mock<IGenericRepo<Enrollment>>();
            var studentRepo = new Mock<IGenericRepo<Student>>();
            var courseRepo = new Mock<IGenericRepo<Course>>();
            var course = new Course { Id = 4, ClassRoom = new ClassRoom { Capacity = 1 }, EnrolledCount = 1 };
            studentRepo.Setup(repo => repo.ReadAsync(3)).ReturnsAsync(new Student { Id = 3 });
            courseRepo.Setup(repo => repo.ReadAsync(4, "ClassRoom")).ReturnsAsync(course);
            enrollmentRepo.Setup(repo => repo.ReadAllAsync(It.IsAny<string>())).ReturnsAsync(new[]
            {
                new Enrollment { Id = 1, StudentId = 5, CourseId = 4, Status = EnrollmentStatus.Enrolled }
            });
            enrollmentRepo.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            var logic = new EnrollmentLogic(enrollmentRepo.Object, studentRepo.Object, courseRepo.Object);
            var result = await logic.EnrollAsync(new EnrollmentCreateDto { StudentId = 3, CourseId = 4 });

            Assert.That(result.Status, Is.EqualTo(EnrollmentStatus.Waitlisted));
            Assert.That(course.EnrolledCount, Is.EqualTo(1));
            courseRepo.Verify(repo => repo.UpdateWithoutSaving(It.IsAny<Course>()), Times.Never);
            enrollmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DropAsync_WhenEnrolledStudentDrops_PromotesOldestWaitlistedStudent()
        {
            var enrollmentRepo = new Mock<IGenericRepo<Enrollment>>();
            var studentRepo = new Mock<IGenericRepo<Student>>();
            var courseRepo = new Mock<IGenericRepo<Course>>();
            var course = new Course { Id = 4, ClassRoom = new ClassRoom { Capacity = 1 }, EnrolledCount = 1 };
            var activeEnrollment = new Enrollment { Id = 10, CourseId = 4, Status = EnrollmentStatus.Enrolled };
            var oldestWaitlisted = new Enrollment
            {
                Id = 11,
                CourseId = 4,
                Status = EnrollmentStatus.Waitlisted,
                CreatedAt = new DateTime(2025, 1, 1)
            };
            var newerWaitlisted = new Enrollment
            {
                Id = 12,
                CourseId = 4,
                Status = EnrollmentStatus.Waitlisted,
                CreatedAt = new DateTime(2025, 2, 1)
            };
            enrollmentRepo.Setup(repo => repo.ReadAsync(10)).ReturnsAsync(activeEnrollment);
            courseRepo.Setup(repo => repo.ReadAsync(4, "ClassRoom")).ReturnsAsync(course);
            enrollmentRepo.Setup(repo => repo.ReadAllAsync(It.IsAny<string>())).ReturnsAsync(new[] { newerWaitlisted, oldestWaitlisted });
            enrollmentRepo.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            var logic = new EnrollmentLogic(enrollmentRepo.Object, studentRepo.Object, courseRepo.Object);
            await logic.DropAsync(10);

            Assert.That(activeEnrollment.Status, Is.EqualTo(EnrollmentStatus.Dropped));
            Assert.That(oldestWaitlisted.Status, Is.EqualTo(EnrollmentStatus.Enrolled));
            Assert.That(newerWaitlisted.Status, Is.EqualTo(EnrollmentStatus.Waitlisted));
            Assert.That(course.EnrolledCount, Is.EqualTo(1));
            enrollmentRepo.Verify(repo => repo.DeleteAsync(10), Times.Never);
            enrollmentRepo.Verify(repo => repo.UpdateWithoutSaving(activeEnrollment), Times.Once);
            enrollmentRepo.Verify(repo => repo.UpdateWithoutSaving(oldestWaitlisted), Times.Once);
            enrollmentRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }
}
