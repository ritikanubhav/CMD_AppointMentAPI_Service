using CMD.Appointment.ApiService.Controllers;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CMD.Test
{
    [TestClass]
    public class GetAppointmentByStatus
    {
        private Mock<IAppointmentRepo> appointmentRepoMock;
        private AppointmentController controller;
        private DateValidator dateValidatorMock;

        [TestInitialize]
        public void Setup()
        {
            appointmentRepoMock = new Mock<IAppointmentRepo>();

            // Set up mock behavior for different scenarios
            appointmentRepoMock.Setup(repo => repo.FilterAppointmentsByStatus(AppointmentStatus.SCHEDULED.ToString(), It.IsAny<int>(), It.IsAny<int>()))
                                .ReturnsAsync(new List<AppointmentModel>
                                {
                                new AppointmentModel
                                {
                                    Id = 1,
                                    PurposeOfVisit = "Checkup",
                                    Date = DateOnly.FromDateTime(DateTime.Now),
                                    Time = TimeOnly.FromDateTime(DateTime.Now.AddHours(2)),
                                    Email = "patient1@example.com",
                                    Phone = "123-456-7890",
                                    Status = AppointmentStatus.SCHEDULED,
                                    Message = "General checkup",
                                    CreatedBy = "admin",
                                    CreatedDate = DateTime.Now,
                                    LastModifiedBy = "admin",
                                    LastModifiedDate = DateTime.Now,
                                    PatientId = 1,
                                    DoctorId = 1
                                }
                                });

            appointmentRepoMock.Setup(repo => repo.FilterAppointmentsByStatus(AppointmentStatus.CANCELLED.ToString(), It.IsAny<int>(), It.IsAny<int>()))
                                .ReturnsAsync(new List<AppointmentModel>());

            appointmentRepoMock.Setup(repo => repo.FilterAppointmentsByStatus(It.Is<string>(s => string.IsNullOrEmpty(s)), It.IsAny<int>(), It.IsAny<int>()))
                                .ReturnsAsync(new List<AppointmentModel>());

            controller = new AppointmentController(appointmentRepoMock.Object,dateValidatorMock);
        }

        [TestMethod]
        public async Task FilterAppointmentsByStatus_ReturnsOk_WhenAppointmentsFound()
        {
            var status = AppointmentStatus.SCHEDULED.ToString();
            // Act
            var result = await controller.FilterAppointmentsByStatus(status);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var resultAppointments = okResult.Value as List<AppointmentModel>;
            Assert.IsNotNull(resultAppointments);
            Assert.AreEqual(1, resultAppointments.Count);
            Assert.AreEqual(AppointmentStatus.SCHEDULED, resultAppointments[0].Status);
        }

        [TestMethod]
        public async Task FilterAppointmentsByStatus_ReturnsNotFound_WhenNoAppointmentsFound()
        {
            // Arrange
            var status = AppointmentStatus.CANCELLED.ToString();

            // Act
            var result = await controller.FilterAppointmentsByStatus(status);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual($"No appointments found with status: {status}", notFoundResult.Value);
        }

        [TestMethod]
        public async Task FilterAppointmentsByStatus_ReturnsNotFound_WhenStatusIsNull()
        {
            // Arrange
            string status = null;

            // Act
            var result = await controller.FilterAppointmentsByStatus(status);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual($"No appointments found with status: {status}", notFoundResult.Value);
        }

        [TestMethod]
        public async Task FilterAppointmentsByStatus_ReturnsBadRequest_OnInvalidStatus()
        {
            // Arrange
            string status = ""; // Empty status (could represent invalid input)

            // Act
            var result = await controller.FilterAppointmentsByStatus(status);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual($"No appointments found with status: {status}", notFoundResult.Value);
        }
    }
}