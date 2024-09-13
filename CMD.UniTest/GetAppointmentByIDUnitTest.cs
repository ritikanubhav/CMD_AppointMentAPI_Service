using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CMD.Appointment.ApiService.Controllers;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Manager;
using CMD.Appointment.Domain.Services; // Make sure to import this

namespace CMD.Test
{
    [TestClass]
    public class AppointmentControllerTests
    {
        private Mock<IAppointmentManager> _mockManager;
        private Mock<IMessageService> _mockMessageService; // Add this mock
        private AppointmentController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockManager = new Mock<IAppointmentManager>();
            _mockMessageService = new Mock<IMessageService>(); // Initialize the mock
            _controller = new AppointmentController(_mockManager.Object, _mockMessageService.Object); // Pass both mocks
        }

        // Test case: Appointment found, should return 200 OK
        [TestMethod]
        public async Task GetAppointmentById_ShouldReturnOk_WhenAppointmentIsFound()
        {
            // Arrange
            var appointmentId = 1;
            var mockAppointment = new AppointmentModel
            {
                Id = appointmentId,
                PurposeOfVisit = "Checkup",
                Date = DateOnly.FromDateTime(DateTime.Now),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                Email = "test@example.com",
                Phone = "1234567890",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Test message",
                PatientId = 1,
                DoctorId = 1
            };

            _mockManager.Setup(manager => manager.GetAppointmentById(appointmentId))
                        .ReturnsAsync(mockAppointment);

            // Act
            var result = await _controller.GetAppointmentById(appointmentId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(mockAppointment, result.Value);
        }

        // Test case: Appointment not found, should return 404 NotFound
        [TestMethod]
        public async Task GetAppointmentById_ShouldReturnNotFound_WhenAppointmentIsNotFound()
        {
            // Arrange
            var appointmentId = 1;
            _mockManager.Setup(manager => manager.GetAppointmentById(appointmentId))
                        .ReturnsAsync((AppointmentModel)null);

            // Act
            var result = await _controller.GetAppointmentById(appointmentId) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
        }

        // Test case: Exception thrown, should return 400 BadRequest with error message
        [TestMethod]
        public async Task GetAppointmentById_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var appointmentId = 1;
            var exceptionMessage = "Something went wrong";

            _mockManager.Setup(manager => manager.GetAppointmentById(appointmentId))
                        .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetAppointmentById(appointmentId) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual(exceptionMessage, result.Value);
        }
    }
}
