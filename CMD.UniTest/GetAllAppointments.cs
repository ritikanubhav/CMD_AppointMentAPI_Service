//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using CMD.Appointment.ApiService.Controllers;
//using CMD.Appointment.Domain.Entities;
//using CMD.Appointment.Domain.Enums;
//using CMD.Appointment.Domain.Manager;
//using AutoMapper;
//using CMD.Appointment.Domain.Services;

//namespace CMD.UnitTest
//{
//    [TestClass]
//    public class AppointmentControllerTests
//    {
//        private Mock<IAppointmentManager> _mockManager;
//        private AppointmentController _controller;
//        private Mock<IMapper> _mockMapper;
//        private Mock<IMessageService> _messageService;

//        [TestInitialize]
//        public void Setup()
//        {
//            _mockManager = new Mock<IAppointmentManager>();
//            _controller = new AppointmentController(null, _mockManager.Object);
//        }

//        // Test case: Valid pagination, appointments found, should return 200 OK
//        [TestMethod]
//        public async Task GetAllAppointments_ShouldReturnOk_WhenAppointmentsAreFound()
//        {
//            // Arrange
//            var pageNo = 1;
//            var pageLimit = 10;
//            var mockAppointments = new List<AppointmentModel>
//            {
//                new AppointmentModel { Id = 1, PurposeOfVisit = "Checkup", Date = DateOnly.FromDateTime(DateTime.Now), Time = TimeOnly.FromDateTime(DateTime.Now), Email = "test@example.com", Phone = "1234567890", Status = AppointmentStatus.SCHEDULED, Message = "Test message", CreatedBy = "Admin", CreatedDate = DateTime.Now, PatientId = 1, DoctorId = 1 },
//                new AppointmentModel { Id = 2, PurposeOfVisit = "Consultation", Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), Time = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)), Email = "test2@example.com", Phone = "0987654321", Status = AppointmentStatus.CANCELLED, Message = "Another test message", CreatedBy = "Admin", CreatedDate = DateTime.Now, PatientId = 2, DoctorId = 2 }
//            };

//            _mockManager.Setup(manager => manager.GetAllAppointments(pageNo, pageLimit))
//                        .ReturnsAsync(mockAppointments);

//            // Act
//            var result = await _controller.GetAllAppointments(pageNo, pageLimit) as OkObjectResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
//            Assert.AreEqual(mockAppointments, result.Value);
//        }

//        // Test case: No appointments found, should return 404 NotFound
//        [TestMethod]
//        public async Task GetAllAppointments_ShouldReturnNotFound_WhenNoAppointmentsAreFound()
//        {
//            // Arrange
//            var pageNo = 1;
//            var pageLimit = 10;
//            var mockAppointments = new List<AppointmentModel>();

//            _mockManager.Setup(manager => manager.GetAllAppointments(pageNo, pageLimit))
//                        .ReturnsAsync(mockAppointments);

//            // Act
//            var result = await _controller.GetAllAppointments(pageNo, pageLimit) as NotFoundObjectResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
//            Assert.AreEqual("No appointments found", result.Value);
//        }

//        // Test case: Invalid pagination, should handle gracefully and return appointments
//        [TestMethod]
//        public async Task GetAllAppointments_ShouldReturnOk_WhenInvalidPaginationIsGiven()
//        {
//            // Arrange
//            var pageNo = -1; // Invalid page number
//            var pageLimit = 0; // Invalid page limit
//            var mockAppointments = new List<AppointmentModel>
//            {
//                new AppointmentModel { Id = 1, PurposeOfVisit = "Checkup", Date = DateOnly.FromDateTime(DateTime.Now), Time = TimeOnly.FromDateTime(DateTime.Now), Email = "test@example.com", Phone = "1234567890", Status = AppointmentStatus.SCHEDULED, Message = "Test message", CreatedBy = "Admin", CreatedDate = DateTime.Now, PatientId = 1, DoctorId = 1 }
//            };

//            _mockManager.Setup(manager => manager.GetAllAppointments(pageNo, pageLimit))
//                        .ReturnsAsync(mockAppointments);

//            // Act
//            var result = await _controller.GetAllAppointments(pageNo, pageLimit) as OkObjectResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
//            Assert.AreEqual(mockAppointments, result.Value);
//        }

//        // Test case: Exception thrown, should return 400 BadRequest
//        [TestMethod]
//        public async Task GetAllAppointments_ShouldReturnBadRequest_WhenExceptionIsThrown()
//        {
//            // Arrange
//            var pageNo = 1;
//            var pageLimit = 10;
//            var exceptionMessage = "An error occurred";

//            _mockManager.Setup(manager => manager.GetAllAppointments(pageNo, pageLimit))
//                        .ThrowsAsync(new Exception(exceptionMessage));

//            // Act
//            var result = await _controller.GetAllAppointments(pageNo, pageLimit) as BadRequestObjectResult;

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
//            Assert.AreEqual(exceptionMessage, result.Value);
//        }
//    }
//}
