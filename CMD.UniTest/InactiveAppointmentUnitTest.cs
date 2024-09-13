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

[TestClass]
public class AppointmentManagerInactiveTests
{

    private Mock<IAppointmentRepo> _mockRepo;
    private AppointmentManager _appointmentManager;

    private Mock<IMapper> _mockMapper;
    private Mock<IMessageService> _messageService;
    [TestInitialize]
    public void Setup()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepo = new Mock<IAppointmentRepo>();
        _messageService = new Mock<IMessageService>();
        _appointmentManager = new AppointmentManager(_mockRepo.Object, _mockMapper.Object, _messageService.Object);
    }

    [TestMethod]
    public async Task GetInactiveAppointments_ValidPagination_ReturnsAppointments()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 20;
        var expectedAppointments = new List<AppointmentModel>
        {
            new AppointmentModel { Status = AppointmentStatus.CANCELLED },
            new AppointmentModel { Status = AppointmentStatus.CLOSED },
            new AppointmentModel { Status = AppointmentStatus.CLOSED },

        };

        _mockRepo.Setup(repo => repo.GetInactiveAppointments(pageNumber, pageSize))
            .ReturnsAsync(expectedAppointments);

        // Act
        var result = await _appointmentManager.GetInactiveAppointments(pageNumber, pageSize);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedAppointments.Count, result.Count);
        CollectionAssert.AreEqual(expectedAppointments, result);
    }

    [TestMethod]
    public async Task GetInactiveAppointments_NoAppointments_ReturnsEmptyList()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 20;

        _mockRepo.Setup(repo => repo.GetInactiveAppointments(pageNumber, pageSize))
            .ReturnsAsync(new List<AppointmentModel>());

        // Act
        var result = await _appointmentManager.GetInactiveAppointments(pageNumber, pageSize);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(NotValidPaginationException))]
    public async Task GetInactiveAppointments_InvalidPageNumber_ThrowsException()
    {
        // Arrange
        int pageNumber = 0;
        int pageSize = 20;

        // Act
        await _appointmentManager.GetInactiveAppointments(pageNumber, pageSize);

        // Assert is handled by ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(NotValidPaginationException))]
    public async Task GetInactiveAppointments_InvalidPageSize_ThrowsException()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 101;

        // Act
        await _appointmentManager.GetInactiveAppointments(pageNumber, pageSize);

        // Assert is handled by ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(NotValidPaginationException))]
    public async Task GetInactiveAppointments_NegativePageSize_ThrowsException()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = -1;

        // Act
        await _appointmentManager.GetInactiveAppointments(pageNumber, pageSize);

        // Assert is handled by ExpectedException
    }
}