using System.Net.NetworkInformation;
using CMD.Appointment.ApiService.Controllers;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace CMD.UnitTest
{
    [TestClass]
    public class EditAppointmentUnitTest
    {
        private Mock<IAppointmentRepo> appointmentRepoMock;
        private AppointmentController controller;
        private IDateValidator validator;

        [TestInitialize]
        public void Setup()
        {
            validator = new DateValidator();
            // Create a mock repository
            appointmentRepoMock = new Mock<IAppointmentRepo>();

            // Create an instance of the controller with the mock repository
            controller = new AppointmentController(appointmentRepoMock.Object,validator);
        }

        [TestMethod]
        public async Task UpdateAppointment_ReturnsOkResult_WithValidAppointment()
        {
            // Arrange
            var appointment = new AppointmentModel
            {
                Id = 1,
                PurposeOfVisit = "General Checkup",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                Email = "test@example.com",
                Phone = "1234567890",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Looking forward to the appointment",
                CreatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                LastModifiedBy = "admin",
                LastModifiedDate = DateTime.UtcNow,
                PatientId = 101,
                DoctorId = 501
            };

            // Mock UpdateAppointment to complete successfully
            appointmentRepoMock.Setup(repo => repo.UpdateAppointment(appointment))
                                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.UpdateAppointment(appointment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(appointment, okResult.Value);
        }

        [TestMethod]
        public async Task UpdateAppointment_ReturnsBadRequest_OnException()
        {
            // Arrange
            var appointment = new AppointmentModel
            {
                Id = 1,
                PurposeOfVisit = "General Checkup",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                Email = "test@example.com",
                Phone = "1234567890",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Looking forward to the appointment",
                CreatedBy = "admin",
                CreatedDate = DateTime.UtcNow,
                LastModifiedBy = "admin",
                LastModifiedDate = DateTime.UtcNow,
                PatientId = 101,
                DoctorId = 501
            };

            // Mock UpdateAppointment to throw an exception
            appointmentRepoMock.Setup(repo => repo.UpdateAppointment(appointment))
                                .ThrowsAsync(new Exception("Error updating appointment"));

            // Act
            var result = await controller.UpdateAppointment(appointment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Error updating appointment", badRequestResult.Value);
        }
    }
}