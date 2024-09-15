using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CMD.Appointment.ApiService.Controllers;
using CMD.Appointment.Domain.Manager;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.Services;
using CMD.Appointment.Domain.Exceptions;

namespace CMD.UnitTest
{
    /// <summary>
    /// Contains unit tests for the cancellation feature of the <see cref="AppointmentController"/>.
    /// </summary>
    [TestClass]
    public class CancellationFeature
    {
        private Mock<IAppointmentManager> _mockAppointmentManager;
        private Mock<IMessageService> _mockMessageService;
        private AppointmentController _controller;

        /// <summary>
        /// Initializes the test setup, including mocks and the controller instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Initialize mocks
            _mockAppointmentManager = new Mock<IAppointmentManager>();
            _mockMessageService = new Mock<IMessageService>();

            // Initialize controller with mocks
            _controller = new AppointmentController(_mockAppointmentManager.Object, _mockMessageService.Object);
        }

        /// <summary>
        /// Tests that the <see cref="AppointmentController.CancelAppointment(int)"/> method returns an <see cref="OkObjectResult"/> with a success message when the appointment is successfully canceled.
        /// </summary>
        [TestMethod]
        public async Task CancelAppointment_ReturnsNoContent_OnValidId()
        {
            // Arrange
            int appointmentId = 1;

            // Mock the manager method to simulate success
            _mockAppointmentManager.Setup(m => m.CancelAppointment(appointmentId))
                .Returns(Task.CompletedTask);

            // Mock message service response
            _mockMessageService.Setup(m => m.GetMessage("CompletedCancellation"))
                .Returns("Cancellation completed successfully");

            // Act
            var result = await _controller.CancelAppointment(appointmentId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result, "Expected OkObjectResult but got null.");
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode, "Expected status code 200.");
            Assert.AreEqual("Cancellation completed successfully", result.Value, "Expected success message did not match.");

            // Verify that the manager method was called with the correct id
            _mockAppointmentManager.Verify(m => m.CancelAppointment(appointmentId), Times.Once);
        }

        /// <summary>
        /// Tests that the <see cref="AppointmentController.CancelAppointment(int)"/> method returns a <see cref="NotFoundObjectResult"/> with an error message when the appointment is not found.
        /// </summary>
        [TestMethod]
        public async Task CancelAppointment_ReturnsNotFound_OnAppointmentNotFound()
        {
            // Arrange
            int appointmentId = 1; // Valid ID but appointment not found
            _mockAppointmentManager.Setup(m => m.CancelAppointment(It.IsAny<int>()))
                .Throws(new NotFoundException("There is no appointment with the appointmentId 1"));

            // Act
            var result = await _controller.CancelAppointment(appointmentId) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result, "Expected NotFoundObjectResult but got null.");
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode, "Expected status code 404.");
            Assert.AreEqual("There is no appointment with the appointmentId 1", result.Value, "Expected error message did not match.");
        }

        /// <summary>
        /// Tests that the <see cref="AppointmentController.CancelAppointment(int)"/> method returns a <see cref="ObjectResult"/> with a bad request error message when an invalid appointment ID is provided.
        /// </summary>
        [TestMethod]
        public async Task CancelAppointment_ReturnsBadRequest_OnInvalidId()
        {
            // Arrange
            int appointmentId = -1; // Invalid ID

            // Mocking the exception thrown for invalid ID scenario
            _mockAppointmentManager.Setup(m => m.CancelAppointment(It.IsAny<int>()))
                .Throws(new ArgumentException("Invalid appointment ID"));

            // Act
            var result = await _controller.CancelAppointment(appointmentId) as ObjectResult;

            // Assert
            Assert.IsNotNull(result, "Expected ObjectResult but got null.");
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode, "Expected status code 500.");
            Assert.AreEqual("Invalid appointment ID", result.Value, "Expected error message did not match.");
        }
    }
}
