using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CMD.Appointment.ApiService.Controllers;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.Manager;
using CMD.Appointment.Domain.Services;

namespace CMD.UnitTest
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
            // Initialize mocks
            _mockManager = new Mock<IAppointmentManager>();
            _mockMessageService = new Mock<IMessageService>();

            // Initialize controller with mocks
            _controller = new AppointmentController(_mockManager.Object, _mockMessageService.Object);
        }

        /// <summary>
        /// Tests that the <see cref="AppointmentController.GetAllAppointments"/> method returns an <see cref="OkObjectResult"/>
        /// with the appointments data when appointments are found.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task GetAllAppointments_ShouldReturnOk_WhenAppointmentsAreFound()
        {
            // Arrange
            var pageNo = 1;
            var pageLimit = 10;
            var mockAppointments = new List<AppointmentModel>
            {
                new AppointmentModel { Id = 1, PurposeOfVisit = "Checkup", Date = DateOnly.FromDateTime(DateTime.Now), Time = TimeOnly.FromDateTime(DateTime.Now), Email = "test@example.com", Phone = "+9198989898", Status = AppointmentStatus.SCHEDULED, Message = "Test message", CreatedBy = "Admin", CreatedDate = DateTime.Now, PatientId = 1, DoctorId = 1 },
                new AppointmentModel { Id = 2, PurposeOfVisit = "Consultation", Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), Time = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)), Email = "test2@example.com", Phone = "+91987654321", Status = AppointmentStatus.CANCELLED, Message = "Another test message", CreatedBy = "Admin", CreatedDate = DateTime.Now, PatientId = 2, DoctorId = 2 }
            };
            var mockResponse = new AppointmentResponse
            {
                TotalAppointments = 2,
                PageLimit = pageLimit,
                PageNumber = pageNo,
                Items = mockAppointments
            };

            _mockManager.Setup(manager => manager.GetAllAppointments(pageNo, pageLimit))
                        .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAllAppointments(pageNo, pageLimit) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(mockResponse, result.Value);
        }

        /// <summary>
        /// Tests that the <see cref="AppointmentController.GetAllAppointments"/> method returns a <see cref="NotFoundObjectResult"/>
        /// with a message when no appointments are found.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task GetAllAppointments_ShouldReturnNotFound_WhenNoAppointmentsAreFound()
        {
            // Arrange
            var pageNo = 1;
            var pageLimit = 10;
            var mockAppointments = new List<AppointmentModel>();

            var mockResponse = new AppointmentResponse
            {
                TotalAppointments = 0,
                PageLimit = pageLimit,
                PageNumber = pageNo,
                Items = mockAppointments
            };

            _mockManager.Setup(manager => manager.GetAllAppointments(pageNo, pageLimit))
                        .ReturnsAsync(mockResponse);

            _mockMessageService.Setup(service => service.GetMessage(It.IsAny<string>()))
                               .Returns("No appointments found");

            // Act
            var result = await _controller.GetAllAppointments(pageNo, pageLimit) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual("No appointments found", result.Value);
        }

        /// <summary>
        /// Tests that the <see cref="AppointmentController.GetAllAppointments"/> method returns an <see cref="OkObjectResult"/>
        /// with the appointments data even when invalid pagination parameters are provided.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task GetAllAppointments_ShouldReturnOk_WhenInvalidPaginationIsGiven()
        {
            // Arrange
            var pageNo = -1; // Invalid page number
            var pageLimit = 0; // Invalid page limit
            var mockAppointments = new List<AppointmentModel>
            {
                new AppointmentModel { Id = 1, PurposeOfVisit = "Checkup", Date = DateOnly.FromDateTime(DateTime.Now), Time = TimeOnly.FromDateTime(DateTime.Now), Email = "test@example.com", Phone = "1234567890", Status = AppointmentStatus.SCHEDULED, Message = "Test message", CreatedBy = "Admin", CreatedDate = DateTime.Now, PatientId = 1, DoctorId = 1 }
            };

            var mockResponse = new AppointmentResponse
            {
                TotalAppointments = 1,
                PageLimit = pageLimit,
                PageNumber = pageNo,
                Items = mockAppointments
            };

            _mockManager.Setup(manager => manager.GetAllAppointments(pageNo, pageLimit))
                        .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAllAppointments(pageNo, pageLimit) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual(mockResponse, result.Value);
        }

        /// <summary>
        /// Tests that the <see cref="AppointmentController.GetAllAppointments"/> method returns a <see cref="BadRequestObjectResult"/>
        /// with an error message when an exception is thrown.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task GetAllAppointments_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var pageNo = 1;
            var pageLimit = 10;
            var exceptionMessage = "An error occurred";

            _mockManager.Setup(manager => manager.GetAllAppointments(pageNo, pageLimit))
                        .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetAllAppointments(pageNo, pageLimit) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.AreEqual(exceptionMessage, result.Value);
        }
    }
}
