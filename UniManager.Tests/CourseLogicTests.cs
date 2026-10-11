using Moq;
using UniManager.Logic.Logic;
using UniManager.Models.DTOs;
using UniManager.Models.Models;
using UniManager.Repository;

namespace UniManager.Tests
{
    [TestFixture]
    public class CourseLogicTests
    {
        [Test]
        public void UpdateAsync_WhenNewClassroomIsTooSmall_ThrowsConflictException()
        {
            var courseRepo = new Mock<IGenericRepo<Course>>();
            var teacherRepo = new Mock<IGenericRepo<Teacher>>();
            var classroomRepo = new Mock<IGenericRepo<ClassRoom>>();
            var course = new Course { Id = 9, ClassRoomId = 1, EnrolledCount = 4 };
            teacherRepo.Setup(repo => repo.ReadAsync(5)).ReturnsAsync(new Teacher { Id = 5 });
            classroomRepo.Setup(repo => repo.ReadAsync(2)).ReturnsAsync(new ClassRoom { Id = 2, Capacity = 3 });
            courseRepo.Setup(repo => repo.ReadAsync(9)).ReturnsAsync(course);
            var logic = new CourseLogic(courseRepo.Object, teacherRepo.Object, classroomRepo.Object);

            Assert.ThrowsAsync<InvalidOperationException>(() => logic.UpdateAsync(9, new CourseUpdateDto
            {
                Title = "New room course",
                Credits = 3,
                TeacherId = 5,
                ClassRoomId = 2
            }));
            courseRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Course>()), Times.Never);
        }
    }
}
