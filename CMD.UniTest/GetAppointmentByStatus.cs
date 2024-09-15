using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CMD.Appointment.ApiService.Controllers;
using CMD.Appointment.Domain.DTO;
using CMD.Appointment.Domain.Services;
using CMD.Appointment.Domain.Manager;

namespace CMD.UnitTest
{
    [TestClass]
    public class AppointmentControllerStatusTests
    {
        private Mock<IAppointmentManager> _appointmentManagerMock;
        private Mock<IMessageService> _messageServiceMock;
        private AppointmentController _controller;

        /// <summary>
        /// Initializes the test environment by creating mocks and controller instance.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _appointmentManagerMock = new Mock<IAppointmentManager>();
            _messageServiceMock = new Mock<IMessageService>(); // Create mock for IMessageService

            _controller = new AppointmentController(_appointmentManagerMock.Object, _messageServiceMock.Object);
        }

        /// <summary>
        /// Tests that the <see cref="AppointmentController.UpdateAppointment"/> method returns an <see cref="OkObjectResult"/>
        /// with the updated appointment data when valid data is provided.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task UpdateAppointment_ReturnsOkResult_WithValidData()
        {
            // Arrange
            var appointmentData = new UpdateAppointmentDTO
            {
                PurposeOfVisit = "General Checkup",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                Email = "test@example.com",
                Phone = "1234567890",
                Message = "Looking forward to the appointment",
                LastModifiedBy = "admin"
            };
            var id = 1;

            // Mock UpdateAppointment to complete successfully
            _appointmentManagerMock.Setup(manager => manager.UpdateAppointment(appointmentData, id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateAppointment(appointmentData, id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(appointmentData, okResult.Value);
        }

        /// <summary>
        /// Tests that the <see cref="AppointmentController.UpdateAppointment"/> method returns a <see cref="BadRequestObjectResult"/>
        /// when the model state is invalid.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task UpdateAppointment_ReturnsBadRequest_OnInvalidModelState()
        {
            // Arrange
            _controller.ModelState.AddModelError("PurposeOfVisit", "Required");
            var appointmentData = new UpdateAppointmentDTO
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                Email = "test@example.com",
                Phone = "1234567890",
                Message = "Looking forward to the appointment",
                LastModifiedBy = "admin"
            };
            var id = 1;

            // Act
            var result = await _controller.UpdateAppointment(appointmentData, id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);

            // Verify that the result is a SerializableError
            var serializableError = badRequestResult.Value as SerializableError;
            Assert.IsNotNull(serializableError);
            Assert.IsTrue(serializableError.ContainsKey("PurposeOfVisit"));

            var errorMessages = serializableError["PurposeOfVisit"] as IEnumerable<string>;
            Assert.IsNotNull(errorMessages);
            Assert.AreEqual("Required", errorMessages.First());
        }

        /// <summary>
        /// Tests that the <see cref="AppointmentController.UpdateAppointment"/> method returns a <see cref="BadRequestObjectResult"/>
        /// when an exception is thrown during the update process.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task UpdateAppointment_ReturnsBadRequest_OnException()
        {
            // Arrange
            var appointmentData = new UpdateAppointmentDTO
            {
                PurposeOfVisit = "General Checkup",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                Email = "test@example.com",
                Phone = "1234567890",
                Message = "Looking forward to the appointment",
                LastModifiedBy = "admin"
            };
            var id = 1;

            // Mock UpdateAppointment to throw an exception
            _appointmentManagerMock.Setup(manager => manager.UpdateAppointment(appointmentData, id))
                .ThrowsAsync(new Exception("Error updating appointment"));

            // Act
            var result = await _controller.UpdateAppointment(appointmentData, id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Error updating appointment", badRequestResult.Value);
        }
    }
}
