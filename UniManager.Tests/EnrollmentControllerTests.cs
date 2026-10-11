using Microsoft.AspNetCore.Mvc;
using Moq;
using UniManager.Logic.Interfaces;
using UniManager.Models.DTOs;
using UniManager.Models.Models;
using UniManagerApi.Controllers;

namespace UniManager.Tests
{
    [TestFixture]
    public class EnrollmentControllerTests
    {
        [Test]
        public async Task Create_ReturnsCreatedResponseContainingFinalEnrollmentStatus()
        {
            var enrollment = new Enrollment
            {
                Id = 17,
                StudentId = 3,
                CourseId = 4,
                Status = EnrollmentStatus.Waitlisted
            };
            var logic = new Mock<IEnrollmentLogic>();
            logic.Setup(service => service.EnrollAsync(It.IsAny<EnrollmentCreateDto>())).ReturnsAsync(enrollment);
            var controller = new EnrollmentController(logic.Object);

            var result = await controller.Create(new EnrollmentCreateDto { StudentId = 3, CourseId = 4 });

            var created = result as CreatedAtActionResult;
            Assert.That(created, Is.Not.Null);
            Assert.That(created.StatusCode, Is.EqualTo(201));
            Assert.That(created.ActionName, Is.EqualTo(nameof(EnrollmentController.Read)));
            Assert.That(created.RouteValues?["id"], Is.EqualTo(enrollment.Id));
            Assert.That(created.Value, Is.SameAs(enrollment));
            Assert.That(((Enrollment)created.Value!).Status, Is.EqualTo(EnrollmentStatus.Waitlisted));
        }

        [Test]
        public async Task Delete_CallsDropInsteadOfHardDelete()
        {
            var logic = new Mock<IEnrollmentLogic>();
            var controller = new EnrollmentController(logic.Object);

            var result = await controller.Delete(17);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            logic.Verify(service => service.DropAsync(17), Times.Once);
        }

        [Test]
        public async Task Create_WhenStudentAlreadyEnrolled_ReturnsConflict()
        {
            var logic = new Mock<IEnrollmentLogic>();
            logic.Setup(service => service.EnrollAsync(It.IsAny<EnrollmentCreateDto>()))
                .ThrowsAsync(new InvalidOperationException("Already enrolled."));
            var controller = new EnrollmentController(logic.Object);

            var result = await controller.Create(new EnrollmentCreateDto { StudentId = 3, CourseId = 4 });

            Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
            Assert.That(((ConflictObjectResult)result).StatusCode, Is.EqualTo(409));
        }

        [Test]
        public async Task Create_WhenStudentOrCourseIsMissing_ReturnsNotFound()
        {
            var logic = new Mock<IEnrollmentLogic>();
            logic.Setup(service => service.EnrollAsync(It.IsAny<EnrollmentCreateDto>()))
                .ThrowsAsync(new KeyNotFoundException("Resource not found."));
            var controller = new EnrollmentController(logic.Object);

            var result = await controller.Create(new EnrollmentCreateDto { StudentId = 3, CourseId = 4 });

            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(((NotFoundObjectResult)result).StatusCode, Is.EqualTo(404));
        }
    }
}
