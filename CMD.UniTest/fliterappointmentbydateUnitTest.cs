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

[TestClass]
public class AppointmentManagerFilterDateTests
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

    [TestMethod]
    [ExpectedException(typeof(NotValidPaginationException))]
    public async Task FilterAppointmentsByDate_InvalidPageNumber_ThrowsException()
    {
        // Arrange
        var date = new DateOnly(2024, 9, 12);
        int pageNumber = 0;
        int pageSize = 20;

        // Act
        await _appointmentManager.FilterAppointmentsByDate(date, pageNumber, pageSize);

        // Assert is handled by ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(NotValidPaginationException))]
    public async Task FilterAppointmentsByDate_InvalidPageSize_ThrowsException()
    {
        // Arrange
        var date = new DateOnly(2024, 9, 12);
        int pageNumber = 1;
        int pageSize = 101;

        // Act
        await _appointmentManager.FilterAppointmentsByDate(date, pageNumber, pageSize);

        // Assert is handled by ExpectedException
    }

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
