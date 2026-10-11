using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using UniManager.Logic;
using UniManager.Models;
using UniManager.Models.DTOs;
using UniManager.Repository;

namespace UniManager.Tests
{
    [TestFixture]
    public class StudentLogicTests
    {
        [Test]
        public async Task CreateAsync_ValidDto_CallRepoCreate()
        {
            var mockRepo = new Mock<IGenericRepo<Student>>();
            var studentLogic = new StudentLogic(mockRepo.Object);
            var newStudentDto = new StudentCreateDto
            {
                firstName = "NewStudent",
                lastName = "NewLastStudent",
                EnrollmentDate = DateTime.Now,
            };

            await studentLogic.CreateAsync(newStudentDto);

            mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<Student>()), Times.Once);
        }
        [Test]
        public void UpdateAsync_StudentNotFound_ThrowsExc()
        {
            var mockRepo = new Mock<IGenericRepo<Student>>();
            var studentLogic = new StudentLogic(mockRepo.Object);

            int fakeId = 999;
            var updateDto = new StudentUpdateDto { firstName = "Fake", lastName = "User" };
            mockRepo.Setup(repo => repo.ReadAsync(fakeId)).ReturnsAsync((Student?)null);
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await studentLogic.UpdateAsync(fakeId, updateDto));
            Assert.That(ex.Message, Is.EqualTo($"Student with this ID ({fakeId}) not found!"));
            mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Student>()), Times.Never);
        }
        [Test]
        public async Task UpdateAsync_ValidDto_MapsPropertiesCorrectly()
        {
            var mockRepo = new Mock<IGenericRepo<Student>>();
            var studentLogic = new StudentLogic(mockRepo.Object);

            int testId = 1;
            DateTime originalDate = new DateTime(2023, 9, 1);

            var existingStudent = new Student
            {
                Id = testId,
                firstName = "OldName",
                lastName = "OldLastName",
                EnrollmentDate = originalDate
            };

            mockRepo.Setup(repo => repo.ReadAsync(testId)).ReturnsAsync(existingStudent);

            var updateDto = new StudentUpdateDto
            {
                firstName = "NewName",
                lastName = "NewLastName"
            };

            await studentLogic.UpdateAsync(testId, updateDto);
            mockRepo.Verify(repo => repo.UpdateAsync(It.Is<Student>(s =>
                s.Id == testId &&                     
                s.firstName == "NewName" &&               
                s.lastName == "NewLastName" &&           
                s.EnrollmentDate == originalDate      
            )), Times.Once);
        }
    }
}
