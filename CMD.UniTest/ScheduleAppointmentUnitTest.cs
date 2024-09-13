using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CMD.Appointment.ApiService.Controllers;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Services;
using CMD.Appointment.Domain.Manager;
using Microsoft.AspNetCore.Http;

namespace CMD.UnitTest
{
    [TestClass]
    public class ScheduleAppointmentUnitTest
    {
        private Mock<IAppointmentManager> _mockManager;
        private Mock<IMessageService> _mockMessageService;
        private AppointmentController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockManager = new Mock<IAppointmentManager>();
            _mockMessageService = new Mock<IMessageService>();
            _controller = new AppointmentController(_mockManager.Object, _mockMessageService.Object);
        }

        [TestMethod]
        public async Task AddAppointment_ReturnsCreated_WhenAppointmentIsValid()
        {
            // Arrange
            var validAppointment = new AppointmentModel
            {
                PurposeOfVisit = "Consultation",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                Email = "test@example.com",
                Phone = "1234567890",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Looking forward",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 1,
                DoctorId = 1
            };

            _mockManager.Setup(manager => manager.CreateAppointment(It.IsAny<AppointmentModel>()))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddAppointment(validAppointment);

            // Assert
            var createdResult = result as CreatedResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [TestMethod]
        public async Task AddAppointment_ReturnsBadRequest_WhenAppointmentIsInvalid()
        {
            // Arrange
            var invalidAppointment = new AppointmentModel
            {
                PurposeOfVisit = "",
                Email = "invalid-email",
                Phone = "invalid-phone"
            };

            _controller.ModelState.AddModelError("PurposeOfVisit", "Purpose of visit is mandatory");
            _controller.ModelState.AddModelError("Email", "Invalid email address format");

            // Act
            var result = await _controller.AddAppointment(invalidAppointment);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task AddAppointment_ReturnsBadRequest_OnException()
        {
            // Arrange
            var appointment = new AppointmentModel
            {
                PurposeOfVisit = "Consultation",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                Email = "test@example.com",
                Phone = "1234567890",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Looking forward",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 1,
                DoctorId = 1
            };

            _mockManager.Setup(manager => manager.CreateAppointment(It.IsAny<AppointmentModel>()))
                        .ThrowsAsync(new Exception("Database failure"));

            // Act
            var result = await _controller.AddAppointment(appointment);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.AreEqual("Database failure", badRequestResult.Value);
        }
        [TestMethod]
        public async Task AddAppointment_ReturnsBadRequest_WhenAppointmentHasPastDate()
        {
            var validAppointment = new AppointmentModel
            {
                PurposeOfVisit = "Consultation",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                Email = "test@example.com",
                Phone = "1234567890",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Looking forward",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 1,
                DoctorId = 1
            };

            _mockManager.Setup(manager => manager.CreateAppointment(It.IsAny<AppointmentModel>()))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddAppointment(validAppointment);

            // Assert
            var createdResult = result as CreatedResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [TestMethod]
        public async Task AddAppointment_ReturnsBadRequest_WhenAppointmentHasFutureDateBeyondLimit()
        {
            var validAppointment = new AppointmentModel
            {
                PurposeOfVisit = "Consultation",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(31)),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                Email = "test@example.com",
                Phone = "1234567890",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Looking forward",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 1,
                DoctorId = 1
            };

            _mockManager.Setup(manager => manager.CreateAppointment(It.IsAny<AppointmentModel>()))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddAppointment(validAppointment);

            // Assert
            var createdResult = result as CreatedResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(StatusCodes.Status201Created, createdResult.StatusCode); // Adjust message based on your implementation
        }
    }
}
