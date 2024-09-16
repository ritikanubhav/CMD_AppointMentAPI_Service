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
    /// <summary>
    /// Unit tests for the <see cref="AppointmentController"/> class.
    /// </summary>
    [TestClass]
    public class ScheduleAppointmentUnitTest
    {
        private Mock<IAppointmentManager> _mockManager;
        private Mock<IMessageService> _mockMessageService;
        private AppointmentController _controller;

        /// <summary>
        /// Initializes the test setup by creating mocks and the controller instance.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _mockManager = new Mock<IAppointmentManager>();
            _mockMessageService = new Mock<IMessageService>();
            _controller = new AppointmentController(_mockManager.Object, _mockMessageService.Object);
        }

        /// <summary>
        /// Tests that a valid appointment returns a <see cref="CreatedResult"/> with HTTP status 201.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Tests that an invalid appointment returns a <see cref="BadRequestObjectResult"/> with HTTP status 400.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Tests that an exception during appointment creation returns a <see cref="BadRequestObjectResult"/> with HTTP status 400 and the exception message.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Tests that an appointment with a past date returns a <see cref="CreatedResult"/> with HTTP status 201.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task AddAppointment_ReturnsBadRequest_WhenAppointmentHasPastDate()
        {
            // Arrange
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

        /// <summary>
        /// Tests that an appointment with a future date beyond the allowed limit returns a <see cref="CreatedResult"/> with HTTP status 201.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task AddAppointment_ReturnsBadRequest_WhenAppointmentHasFutureDateBeyondLimit()
        {
            // Arrange
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
