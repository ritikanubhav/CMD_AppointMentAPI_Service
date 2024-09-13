//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using CMD.Appointment.ApiService.Controllers;
//using CMD.Appointment.Domain.Entities;
//using CMD.Appointment.Domain.Enums;
//using CMD.Appointment.Domain.IRepositories;
//using CMD.Appointment.Domain.Services;
//using Microsoft.AspNetCore.Http;

//namespace CMD.Test
//{
//    [TestClass]
//    public class GetAppointmentByStatus
//    {
//        private Mock<IAppointmentRepo> appointmentRepoMock;
//        private Mock<IDateValidator> dateValidatorMock; // Assuming you have an interface for DateValidator
//        private AppointmentController controller;

//        [TestInitialize]
//        public void Setup()
//        {
//            appointmentRepoMock = new Mock<IAppointmentRepo>();
//            dateValidatorMock = new Mock<IDateValidator>(); // Initialize the mock for DateValidator

//            // Set up mock behavior for different scenarios
//            appointmentRepoMock.Setup(repo => repo.FilterAppointmentsByStatus(AppointmentStatus.SCHEDULED.ToString(), It.IsAny<int>(), It.IsAny<int>()))
//                                .ReturnsAsync(new List<AppointmentModel>
//                                {
//                                    new AppointmentModel
//                                    {
//                                        Id = 1,
//                                        PurposeOfVisit = "Checkup",
//                                        Date = DateOnly.FromDateTime(DateTime.Now),
//                                        Time = TimeOnly.FromDateTime(DateTime.Now.AddHours(2)),
//                                        Email = "patient1@example.com",
//                                        Phone = "123-456-7890",
//                                        Status = AppointmentStatus.SCHEDULED,
//                                        Message = "General checkup",
//                                        CreatedBy = "admin",
//                                        CreatedDate = DateTime.Now,
//                                        LastModifiedBy = "admin",
//                                        LastModifiedDate = DateTime.Now,
//                                        PatientId = 1,
//                                        DoctorId = 1
//                                    }
//                                });

//            appointmentRepoMock.Setup(repo => repo.FilterAppointmentsByStatus(AppointmentStatus.CANCELLED.ToString(), It.IsAny<int>(), It.IsAny<int>()))
//                                .ReturnsAsync(new List<AppointmentModel>());

//            appointmentRepoMock.Setup(repo => repo.FilterAppointmentsByStatus(It.Is<string>(s => string.IsNullOrEmpty(s)), It.IsAny<int>(), It.IsAny<int>()))
//                                .ReturnsAsync(new List<AppointmentModel>());

//            controller = new AppointmentController(appointmentRepoMock.Object, dateValidatorMock.Object);
//        }

//        [TestMethod]
//        public async Task FilterAppointmentsByStatus_ReturnsOk_WhenAppointmentsFound()
//        {
//            // Arrange
//            var status = AppointmentStatus.SCHEDULED.ToString();

//            // Act
//            var result = await controller.FilterAppointmentsByStatus(status);

//            // Assert
//            var okResult = result as OkObjectResult;
//            Assert.IsNotNull(okResult);
//            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);

//            var resultAppointments = okResult.Value as List<AppointmentModel>;
//            Assert.IsNotNull(resultAppointments);
//            Assert.AreEqual(1, resultAppointments.Count);
//            Assert.AreEqual(AppointmentStatus.SCHEDULED, resultAppointments[0].Status);
//        }

//        [TestMethod]
//        public async Task FilterAppointmentsByStatus_ReturnsNotFound_WhenNoAppointmentsFound()
//        {
//            // Arrange
//            var status = AppointmentStatus.CANCELLED.ToString();

//            // Act
//            var result = await controller.FilterAppointmentsByStatus(status);

//            // Assert
//            var notFoundResult = result as NotFoundObjectResult;
//            Assert.IsNotNull(notFoundResult);
//            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
//            Assert.AreEqual($"No appointments found with status: {status}", notFoundResult.Value);
//        }

//        [TestMethod]
//        public async Task FilterAppointmentsByStatus_ReturnsNotFound_WhenStatusIsNull()
//        {
//            // Arrange
//            string status = null;

//            // Act
//            var result = await controller.FilterAppointmentsByStatus(status);

//            // Assert
//            var notFoundResult = result as NotFoundObjectResult;
//            Assert.IsNotNull(notFoundResult);
//            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
//            Assert.AreEqual($"No appointments found with status: {status}", notFoundResult.Value);
//        }

//        [TestMethod]
//        public async Task FilterAppointmentsByStatus_ReturnsNotFound_OnInvalidStatus()
//        {
//            // Arrange
//            string status = ""; // Empty status (could represent invalid input)

//            // Act
//            var result = await controller.FilterAppointmentsByStatus(status);

//            // Assert
//            var notFoundResult = result as NotFoundObjectResult;
//            Assert.IsNotNull(notFoundResult);
//            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
//            Assert.AreEqual($"No appointments found with status: {status}", notFoundResult.Value);
//        }
//    }
//}
