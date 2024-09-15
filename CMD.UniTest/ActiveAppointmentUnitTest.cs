using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using CMD.Appointment.Domain.Services;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.Exceptions;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Manager;
using CMD.Appointment.Domain.Entities;
using AutoMapper;

namespace CMD.UnitTest
{
    /// <summary>
    /// Contains unit tests for the <see cref="AppointmentManager"/> class.
    /// </summary>
    [TestClass]
    public class AppointmentManagerTests
    {
        private Mock<IAppointmentRepo> _mockRepo;
        private AppointmentManager _appointmentManager;
        private Mock<IMapper> _mockMapper;
        private Mock<IMessageService> _messageService;

        /// <summary>
        /// Initializes the test setup, including mocks and the instance of <see cref="AppointmentManager"/>.
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
        /// Tests that <see cref="AppointmentManager.GetActiveAppointments(int, int)"/> returns a list of appointments
        /// when valid pagination parameters are provided and there are appointments in the repository.
        /// </summary>
        [TestMethod]
        public async Task GetActiveAppointments_ValidPagination_ReturnsAppointments()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 20;
            var expectedAppointments = new List<AppointmentModel>
            {
                new AppointmentModel { Status = AppointmentStatus.SCHEDULED },
                new AppointmentModel { Status = AppointmentStatus.SCHEDULED }
            };

            _mockRepo.Setup(repo => repo.GetActiveAppointments(pageNumber, pageSize))
                .ReturnsAsync(expectedAppointments);

            // Act
            var result = await _appointmentManager.GetActiveAppointments(pageNumber, pageSize);

            // Assert
            Assert.IsNotNull(result, "Expected result to be not null.");
            Assert.AreEqual(expectedAppointments.Count, result.Count, "Count of appointments did not match.");
            CollectionAssert.AreEqual(expectedAppointments, result, "Appointments list did not match.");
        }

        /// <summary>
        /// Tests that <see cref="AppointmentManager.GetActiveAppointments(int, int)"/> returns an empty list
        /// when there are no appointments in the repository for valid pagination parameters.
        /// </summary>
        [TestMethod]
        public async Task GetActiveAppointments_NoAppointments_ReturnsEmptyList()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 20;

            _mockRepo.Setup(repo => repo.GetActiveAppointments(pageNumber, pageSize))
                .ReturnsAsync(new List<AppointmentModel>());

            // Act
            var result = await _appointmentManager.GetActiveAppointments(pageNumber, pageSize);

            // Assert
            Assert.IsNotNull(result, "Expected result to be not null.");
            Assert.AreEqual(0, result.Count, "Expected appointment list to be empty.");
        }

        /// <summary>
        /// Tests that <see cref="AppointmentManager.GetActiveAppointments(int, int)"/> throws a <see cref="NotValidPaginationException"/>
        /// when the page number is less than or equal to zero.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotValidPaginationException))]
        public async Task GetActiveAppointments_InvalidPageNumber_ThrowsException()
        {
            // Arrange
            int pageNumber = 0;
            int pageSize = 20;

            // Act
            await _appointmentManager.GetActiveAppointments(pageNumber, pageSize);

            // Assert is handled by ExpectedException
        }

        /// <summary>
        /// Tests that <see cref="AppointmentManager.GetActiveAppointments(int, int)"/> throws a <see cref="NotValidPaginationException"/>
        /// when the page size exceeds the maximum allowed value.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotValidPaginationException))]
        public async Task GetActiveAppointments_InvalidPageSize_ThrowsException()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 101;

            // Act
            await _appointmentManager.GetActiveAppointments(pageNumber, pageSize);

            // Assert is handled by ExpectedException
        }

        /// <summary>
        /// Tests that <see cref="AppointmentManager.GetActiveAppointments(int, int)"/> throws a <see cref="NotValidPaginationException"/>
        /// when the page size is negative.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotValidPaginationException))]
        public async Task GetActiveAppointments_NegativePageSize_ThrowsException()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = -1;

            // Act
            await _appointmentManager.GetActiveAppointments(pageNumber, pageSize);

            // Assert is handled by ExpectedException
        }
    }
}
