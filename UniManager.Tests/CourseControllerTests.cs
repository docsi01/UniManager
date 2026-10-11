using Microsoft.AspNetCore.Mvc;
using Moq;
using UniManager.Logic.Interfaces;
using UniManager.Models.DTOs;
using UniManagerApi.Controllers;

namespace UniManager.Tests
{
    [TestFixture]
    public class CourseControllerTests
    {
        [Test]
        public async Task Update_WhenClassroomCapacityWouldBeExceeded_ReturnsConflict()
        {
            var logic = new Mock<ICourseLogic>();
            logic.Setup(service => service.UpdateAsync(9, It.IsAny<CourseUpdateDto>()))
                .ThrowsAsync(new InvalidOperationException("Classroom capacity exceeded."));
            var controller = new CourseController(logic.Object);

            var result = await controller.Update(9, new CourseUpdateDto
            {
                Title = "Course",
                Credits = 3,
                TeacherId = 5,
                ClassRoomId = 2
            });

            Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
            Assert.That(((ConflictObjectResult)result).StatusCode, Is.EqualTo(409));
        }
    }
}
