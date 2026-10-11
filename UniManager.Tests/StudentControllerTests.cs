using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Moq;
using UniManager.Logic;
using UniManager.Models;
using UniManager.Models.DTOs;
using UniManagerApi.Controllers;

namespace UniManager.Tests
{
    [TestFixture]
    public class StudentsControllerTests
    {
        [Test]
        public async Task GetAll_WhenCalled_ReturnsOkObjectResult_WithStudents()
        {
            var mockLogic = new Mock<IStudentLogic>();

            var fakeStudents = new List<Student>
            {
                new Student { Id = 1, firstName = "NewStudent", lastName = "1" },
                new Student { Id = 2, firstName = "NewStudent", lastName = "2" }
            };

            mockLogic.Setup(logic => logic.ReadAllAsync(It.IsAny<string>()))
                     .ReturnsAsync(fakeStudents);

            var controller = new StudentController(mockLogic.Object);

            var result = await controller.GetAll();

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null, "Expected OkObjectResult (HTTP 200)");
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var returnedData = okResult.Value as IEnumerable<Student>;
            Assert.That(returnedData, Is.EqualTo(fakeStudents));
        }

        [Test]
        public async Task GetById_StudentDoesNotExist_ReturnsNotFound()
        {
            var mockLogic = new Mock<IStudentLogic>();
            int fakeId = 99;

            mockLogic.Setup(logic => logic.ReadAsync(fakeId)).ReturnsAsync((Student?)null);

            var controller = new StudentController(mockLogic.Object);

            var result = await controller.Read(fakeId);

            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null, "Expected NotFoundObjectResult (HTTP 404)");
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task Create_InvalidModelState_ReturnsBadRequest()
        {
            var mockLogic = new Mock<IStudentLogic>();
            var controller = new StudentController(mockLogic.Object);
            var invalidDto = new StudentCreateDto { firstName = " ", lastName = " ", EnrollmentDate = default };
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(invalidDto, new ValidationContext(invalidDto), validationResults, validateAllProperties: true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.FirstOrDefault() ?? string.Empty, validationResult.ErrorMessage ?? "Invalid value.");
            }

            var result = await controller.Create(invalidDto);

            Assert.That(result, Is.InstanceOf<ObjectResult>());
            Assert.That(((ObjectResult)result).StatusCode, Is.EqualTo(400));
            mockLogic.Verify(logic => logic.CreateAsync(It.IsAny<StudentCreateDto>()), Times.Never);
        }

        [Test]
        public async Task Create_ValidDto_ReturnsCreatedResponseWithStudent()
        {
            var student = new Student
            {
                Id = 12,
                firstName = "Alex",
                lastName = "Morgan",
                EnrollmentDate = new DateTime(2025, 1, 15)
            };
            var mockLogic = new Mock<IStudentLogic>();
            mockLogic.Setup(logic => logic.CreateAsync(It.IsAny<StudentCreateDto>())).ReturnsAsync(student);
            var controller = new StudentController(mockLogic.Object);

            var result = await controller.Create(new StudentCreateDto
            {
                firstName = student.firstName,
                lastName = student.lastName,
                EnrollmentDate = student.EnrollmentDate
            });

            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(createdResult.ActionName, Is.EqualTo(nameof(StudentController.Read)));
            Assert.That(createdResult.RouteValues?["id"], Is.EqualTo(student.Id));
            Assert.That(createdResult.Value, Is.SameAs(student));
        }
    }
}