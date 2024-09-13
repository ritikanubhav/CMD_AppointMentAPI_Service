using System;
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
    public class AppointmentControllerEditTests
    {
        private Mock<IAppointmentManager> appointmentManagerMock;
        private Mock<IMessageService> messageServiceMock;
        private AppointmentController controller;

        [TestInitialize]
        public void Setup()
        {
            appointmentManagerMock = new Mock<IAppointmentManager>();
            messageServiceMock = new Mock<IMessageService>(); // Create mock for IMessageService

            controller = new AppointmentController(appointmentManagerMock.Object, messageServiceMock.Object);
        }

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
            appointmentManagerMock.Setup(manager => manager.UpdateAppointment(appointmentData, id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.UpdateAppointment(appointmentData, id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(appointmentData, okResult.Value);
        }

        [TestMethod]
        public async Task UpdateAppointment_ReturnsBadRequest_OnInvalidModelState()
        {
            // Arrange
            controller.ModelState.AddModelError("PurposeOfVisit", "Required");
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
            var result = await controller.UpdateAppointment(appointmentData, id);

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
            appointmentManagerMock.Setup(manager => manager.UpdateAppointment(appointmentData, id))
                .ThrowsAsync(new Exception("Error updating appointment"));

            // Act
            var result = await controller.UpdateAppointment(appointmentData, id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Error updating appointment", badRequestResult.Value);
        }
    }
}
