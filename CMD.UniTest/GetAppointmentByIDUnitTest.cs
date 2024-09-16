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
    /// <summary>
    /// Contains unit tests for the <see cref="AppointmentController"/> class.
    /// </summary>
    [TestClass]
    public class AppointmentControllerTests
    {
        private Mock<IAppointmentManager> _mockManager;
        private Mock<IMessageService> _mockMessageService;
        private AppointmentController _controller;

        /// <summary>
        /// Initializes the test environment by creating mocks and controller instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mockManager = new Mock<IAppointmentManager>();
            _mockMessageService = new Mock<IMessageService>(); // Initialize the mock
            _controller = new AppointmentController(_mockManager.Object, _mockMessageService.Object); // Pass both mocks
        }

        /// <summary>
        /// Tests that the <see cref="AppointmentController.GetAppointmentById"/> method returns an <see cref="OkObjectResult"/>
        /// with the appointment data when the appointment is found.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
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

        /// <summary>
        /// Tests that the <see cref="AppointmentController.GetAppointmentById"/> method returns a <see cref="NotFoundResult"/>
        /// when the appointment is not found.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
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

        /// <summary>
        /// Tests that the <see cref="AppointmentController.GetAppointmentById"/> method returns a <see cref="BadRequestObjectResult"/>
        /// with an error message when an exception is thrown.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
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
