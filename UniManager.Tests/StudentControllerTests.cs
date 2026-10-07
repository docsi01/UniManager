using Microsoft.AspNetCore.Mvc;
using Moq;
using UniManager.Logic;
using UniManager.Models;
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
    }
}