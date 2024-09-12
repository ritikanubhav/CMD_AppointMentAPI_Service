//using CMD.Appointment.ApiService.Controllers;
//using CMD.Appointment.Domain.Entities;
//using CMD.Appointment.Domain.Enums;
//using CMD.Appointment.Domain.IRepositories;
//using CMD.Appointment.Domain.Manager;
//using CMD.Appointment.Domain.Services;
//using Microsoft.AspNetCore.Mvc;
//using Moq;

//namespace CMD.UnitTest
//{
//    [TestClass]
//    public class CancellationFeature
//    {
//        private Mock<IAppointmentRepo> _appointmentRepoMock;
//        private AppointmentController _controller;
//        private IAppointmentManager appointmentManager;
//        [TestInitialize]
//        public void Setup()
//        {
//            _appointmentRepoMock = new Mock<IAppointmentRepo>();
//            _controller = new AppointmentController(_appointmentRepoMock.Object);
//        }

//        [TestMethod]
//        public async Task CancelAppointment_ReturnsNoContent_WhenAppointmentFound()
//        {
//            // Arrange
//            int appointmentId = 1;
//            // Mock the method to simulate that the appointment exists
//            _appointmentRepoMock.Setup(repo => repo.CancelAppointment(appointmentId))
//                                .Callback(() =>
//                                {
//                                    // Simulate the appointment being found and updated
//                                    var existingAppointment = new AppointmentModel
//                                    {
//                                        Id = appointmentId,
//                                        Status = AppointmentStatus.SCHEDULED, // Initial status
//                                        PurposeOfVisit = "Checkup",
//                                        Date = DateOnly.FromDateTime(DateTime.Now),
//                                        Time = TimeOnly.FromDateTime(DateTime.Now.AddHours(2)),
//                                        Email = "patient1@example.com",
//                                        Phone = "123-456-7890",
//                                        Message = "General checkup",
//                                        CreatedBy = "admin",
//                                        CreatedDate = DateTime.Now,
//                                        LastModifiedBy = "admin",
//                                        LastModifiedDate = DateTime.Now,
//                                        PatientId = 1,
//                                        DoctorId = 1
//                                    };

//                                    // Simulate status change
//                                    existingAppointment.Status = AppointmentStatus.CANCELLED;
//                                    existingAppointment.LastModifiedDate = DateTime.UtcNow;
//                                })
//                                .Returns(Task.CompletedTask);

//            // Mock the method to return an appointment when it is canceled
//            // Mock the CancelAppointment method to simulate success
//            //_appointmentRepoMock.Setup(repo => repo.CancelAppointment(appointmentId))
//            //                    .Returns(Task.CompletedTask);

//            // Act
//            var result = await _controller.CancelAppointment(appointmentId);

//            // Assert
//            var noContentResult = result as NoContentResult;
//            Assert.IsNotNull(noContentResult, "Getting the null value");
//            Assert.AreEqual(204, noContentResult.StatusCode);

//            // Verify that the repository method was called with the correct id
//            _appointmentRepoMock.Verify(repo => repo.CancelAppointment(appointmentId), Times.Once);
//        }

//        //[TestMethod]
//        //public async Task CancelAppointment_ReturnsNotFound_WhenAppointmentNotFound()
//        //{
//        //    // Arrange
//        //    int appointmentId = 999; // ID that does not exist

//        //    // Mock the method to simulate that no appointment is found
//        //    _appointmentRepoMock.Setup(repo => repo.CancelAppointment(appointmentId))
//        //                        .Callback(() =>
//        //                        {
//        //                            // Simulate the appointment being found and updated
//        //                            var existingAppointment = new AppointmentModel
//        //                            {
//        //                                Id = appointmentId,
//        //                                Status = AppointmentStatus.SCHEDULED, // Initial status
//        //                                PurposeOfVisit = "Checkup",
//        //                                Date = DateOnly.FromDateTime(DateTime.Now),
//        //                                Time = TimeOnly.FromDateTime(DateTime.Now.AddHours(2)),
//        //                                Email = "patient1@example.com",
//        //                                Phone = "123-456-7890",
//        //                                Message = "General checkup",
//        //                                CreatedBy = "admin",
//        //                                CreatedDate = DateTime.Now,
//        //                                LastModifiedBy = "admin",
//        //                                LastModifiedDate = DateTime.Now,
//        //                                PatientId = 1,
//        //                                DoctorId = 1
//        //                            };

//        //                            // Simulate status change
//        //                            existingAppointment.Status = AppointmentStatus.CANCELLED;
//        //                            existingAppointment.LastModifiedDate = DateTime.UtcNow;
//        //                        })
//        //                        .Returns(Task.CompletedTask);

//        //    // Act
//        //    var result = await _controller.CancelAppointment(appointmentId);

//        //    // Assert
//        //    var notFoundResult = result as NotFoundObjectResult;
//        //    Assert.IsNotNull(notFoundResult, "Expected NotFoundObjectResult, but got null.");
//        //    Assert.AreEqual(404, notFoundResult.StatusCode, "Expected status code 404, but got a different status code.");
//        //    Assert.AreEqual("No appointment found with ID 999", notFoundResult.Value, "Expected NotFound message did not match.");
//        //}


//    //    [TestMethod]
//    //    public async Task CancelAppointment_ReturnsBadRequest_OnInvalidId()
//    //    {
//    //        // Arrange
//    //        int appointmentId = -1; // Invalid ID

//    //        // Act
//    //        var result = await _controller.CancelAppointment(appointmentId);

//    //        // Assert
//    //        var badRequestResult = result as BadRequestObjectResult;
//    //        Assert.IsNotNull(badRequestResult, "Expected BadRequestObjectResult, but got null.");
//    //        Assert.AreEqual(400, badRequestResult.StatusCode, "Expected status code 400, but got a different status code.");
//    //        Assert.AreEqual("Invalid appointment ID", badRequestResult.Value, "Expected BadRequest message did not match.");
//    //    }


//    }
//}