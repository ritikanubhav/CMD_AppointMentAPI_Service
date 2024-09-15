using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.Exceptions;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Services;
using CMD.Appointment.Domain.Manager;
using AutoMapper;

namespace CMD.UnitTest
{
    /// <summary>
    /// Contains unit tests for the <see cref="AppointmentManager"/> class, specifically testing the <see cref="AppointmentManager.FilterAppointmentsByDate"/> method.
    /// </summary>
    [TestClass]
    public class AppointmentManagerFilterDateTests
    {
        private Mock<IAppointmentRepo> _mockRepo;
        private AppointmentManager _appointmentManager;
        private Mock<IMapper> _mockMapper;
        private Mock<IMessageService> _messageService;

        /// <summary>
        /// Initializes the test environment by creating mocks and instance of <see cref="AppointmentManager"/>.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepo = new Mock<IAppointmentRepo>();
            _messageService = new Mock<IMessageService>();
            _appointmentManager = new AppointmentManager(_mockRepo.Object, _mockMapper.Object, _messageService.Object);
        }

        /// <summary>
        /// Tests that <see cref="AppointmentManager.FilterAppointmentsByDate"/> returns a list of appointments when valid input is provided.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task FilterAppointmentsByDate_ValidInput_ReturnsAppointments()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)); // Tomorrow's date
            int pageNumber = 1;
            int pageSize = 20;
            var expectedAppointments = new List<AppointmentModel>
            {
                new AppointmentModel { Id = 1, Date = date, Status = AppointmentStatus.SCHEDULED },
                new AppointmentModel { Id = 2, Date = date, Status = AppointmentStatus.SCHEDULED }
            };

            _mockRepo.Setup(repo => repo.FilterAppointmentsByDate(date, pageNumber, pageSize))
                .ReturnsAsync(expectedAppointments);

            // Act
            var result = await _appointmentManager.FilterAppointmentsByDate(date, pageNumber, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedAppointments.Count, result.Count);
            CollectionAssert.AreEqual(expectedAppointments, result);
        }

        /// <summary>
        /// Tests that <see cref="AppointmentManager.FilterAppointmentsByDate"/> returns an empty list when no appointments are found for the given date.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        public async Task FilterAppointmentsByDate_NoAppointments_ReturnsEmptyList()
        {
            // Arrange
            var date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)); // Day after tomorrow
            int pageNumber = 1;
            int pageSize = 20;

            _mockRepo.Setup(repo => repo.FilterAppointmentsByDate(date, pageNumber, pageSize))
                .ReturnsAsync(new List<AppointmentModel>());

            // Act
            var result = await _appointmentManager.FilterAppointmentsByDate(date, pageNumber, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// Tests that <see cref="AppointmentManager.FilterAppointmentsByDate"/> throws a <see cref="NotValidPaginationException"/> when an invalid page number is provided.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        [ExpectedException(typeof(NotValidPaginationException))]
        public async Task FilterAppointmentsByDate_InvalidPageNumber_ThrowsException()
        {
            // Arrange
            var date = new DateOnly(2024, 9, 12);
            int pageNumber = 0; // Invalid page number
            int pageSize = 20;

            // Act
            await _appointmentManager.FilterAppointmentsByDate(date, pageNumber, pageSize);

            // Assert is handled by ExpectedException
        }

        /// <summary>
        /// Tests that <see cref="AppointmentManager.FilterAppointmentsByDate"/> throws a <see cref="NotValidPaginationException"/> when an invalid page size is provided.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        [ExpectedException(typeof(NotValidPaginationException))]
        public async Task FilterAppointmentsByDate_InvalidPageSize_ThrowsException()
        {
            // Arrange
            var date = new DateOnly(2024, 9, 12);
            int pageNumber = 1;
            int pageSize = 101; // Invalid page size

            // Act
            await _appointmentManager.FilterAppointmentsByDate(date, pageNumber, pageSize);

            // Assert is handled by ExpectedException
        }

        /// <summary>
        /// Tests that <see cref="AppointmentManager.FilterAppointmentsByDate"/> throws a <see cref="NotValidDateException"/> when an invalid date is provided.
        /// </summary>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [TestMethod]
        [ExpectedException(typeof(NotValidDateException))]
        public async Task FilterAppointmentsByDate_InvalidDate_ThrowsException()
        {
            // Arrange
            var date = new DateOnly(1899, 12, 31); // Assuming this is an invalid date
            int pageNumber = 1;
            int pageSize = 20;

            // Act
            await _appointmentManager.FilterAppointmentsByDate(date, pageNumber, pageSize);

            // Assert is handled by ExpectedException
        }
    }
}
