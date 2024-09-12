//using CMD.Appointment.ApiService.Controllers;
//using CMD.Appointment.Domain.Entities;
//using CMD.Appointment.Domain.Enums;
//using CMD.Appointment.Domain.IRepositories;
//using CMD.Appointment.Domain.Services;
//using Microsoft.AspNetCore.Mvc;
//using Moq;

//namespace CMD.UnitTest
//{
//    [TestClass]
//    public class ScheduleAppointmentUnitTest
//    {
//        private DateValidator dateValidator;
//        private Mock<IAppointmentRepo> mockRepo;
//        private AppointmentController controller;

//        [TestInitialize]
//        public void TestInitialize()
//        {
//            dateValidator = new DateValidator();
//            mockRepo = new Mock<IAppointmentRepo>();
//            controller = new AppointmentController(mockRepo.Object,dateValidator);
//        }

//        [TestMethod]
//        public async Task AddAppointment_ReturnsCreated_WhenAppointmentIsValid()
//        {
//            // Arrange
//            var validAppointment = new AppointmentModel
//            {
//                PurposeOfVisit = "Consultation",
//                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
//                Time = TimeOnly.FromDateTime(DateTime.Now),
//                Email = "test@example.com",
//                Phone = "1234567890",
//                Status = AppointmentStatus.SCHEDULED,
//                Message = "Looking forward",
//                CreatedBy = "Admin",
//                CreatedDate = DateTime.Now,
//                LastModifiedBy = "Admin",
//                LastModifiedDate = DateTime.Now,
//                PatientId = 1,
//                DoctorId = 1
//            };

//            mockRepo.Setup(repo => repo.AddAppointment(It.IsAny<AppointmentModel>())).Returns(Task.CompletedTask);

//            // Act
//            var result = await controller.AddAppointment(validAppointment);

//            // Assert
//            var createdResult = result as CreatedResult;
//            Assert.IsNotNull(createdResult);
//            Assert.AreEqual(201, createdResult.StatusCode);
//        }

//        [TestMethod]
//        public async Task AddAppointment_ReturnsBadRequest_WhenAppointmentIsInvalid()
//        {
//            // Arrange
//            var invalidAppointment = new AppointmentModel
//            {
//                PurposeOfVisit = "",
//                Email = "invalid-email",
//                Phone = "invalid-phone"
//            };

//            controller.ModelState.AddModelError("PurposeOfVisit", "Purpose of visit is mandatory");
//            controller.ModelState.AddModelError("Email", "Invalid email address format");

//            // Act
//            var result = await controller.AddAppointment(invalidAppointment);

//            // Assert
//            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
//            var badRequestResult = (BadRequestObjectResult)result;
//            Assert.AreEqual(400, badRequestResult.StatusCode);
//        }

//        [TestMethod]
//        public async Task AddAppointment_ReturnsBadRequest_OnException()
//        {
//            // Arrange
//            var appointment = new AppointmentModel
//            {
//                PurposeOfVisit = "Consultation",
//                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
//                Time = TimeOnly.FromDateTime(DateTime.Now),
//                Email = "test@example.com",
//                Phone = "1234567890",
//                Status = AppointmentStatus.SCHEDULED,
//                Message = "Looking forward",
//                CreatedBy = "Admin",
//                CreatedDate = DateTime.Now,
//                LastModifiedBy = "Admin",
//                LastModifiedDate = DateTime.Now,
//                PatientId = 1,
//                DoctorId = 1
//            };

//            mockRepo.Setup(repo => repo.AddAppointment(It.IsAny<AppointmentModel>()))
//                     .Throws(new Exception("Database failure"));

//            // Act
//            var result = await controller.AddAppointment(appointment);

//            // Assert
//            var badRequestResult = result as BadRequestObjectResult;
//            Assert.IsNotNull(badRequestResult);
//            Assert.AreEqual(400, badRequestResult.StatusCode);
//            Assert.AreEqual("Database failure", badRequestResult.Value);
//        }

//        [TestMethod]
//        public async Task AddAppointment_ReturnsCreated_WhenAppointmentIsValidWithValidDate()
//        {
//            // Arrange: Valid appointment with valid date (today or within 30 days from today)
//            var validAppointment = new AppointmentModel
//            {
//                PurposeOfVisit = "Consultation",
//                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(5)), // Valid Date
//                Time = TimeOnly.FromDateTime(DateTime.Now),
//                Email = "test@example.com",
//                Phone = "1234567890",
//                Status = AppointmentStatus.SCHEDULED,
//                Message = "Looking forward",
//                CreatedBy = "Admin",
//                CreatedDate = DateTime.Now,
//                LastModifiedBy = "Admin",
//                LastModifiedDate = DateTime.Now,
//                PatientId = 1,
//                DoctorId = 1
//            };

//            mockRepo.Setup(repo => repo.AddAppointment(It.IsAny<AppointmentModel>())).Returns(Task.CompletedTask);

//            // Act
//            var result = await controller.AddAppointment(validAppointment);

//            // Assert
//            var createdResult = result as CreatedResult;
//            Assert.IsNotNull(createdResult);
//            Assert.AreEqual(201, createdResult.StatusCode);
//        }

//        [TestMethod]
//        public async Task AddAppointment_ReturnsBadRequest_WhenAppointmentHasPastDate()
//        {
//            // Arrange: Invalid appointment with a past date
//            var pastDateAppointment = new AppointmentModel
//            {
//                PurposeOfVisit = "Consultation",
//                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), // Invalid Date (Past)
//                Time = TimeOnly.FromDateTime(DateTime.Now),
//                Email = "test@example.com",
//                Phone = "1234567890",
//                Status = AppointmentStatus.SCHEDULED,
//                Message = "Looking forward",
//                CreatedBy = "Admin",
//                CreatedDate = DateTime.Now,
//                LastModifiedBy = "Admin",
//                LastModifiedDate = DateTime.Now,
//                PatientId = 1,
//                DoctorId = 1
//            };

//            // Act
//            var result = await controller.AddAppointment(pastDateAppointment);

//            // Assert
//            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
//            var badRequestResult = (BadRequestObjectResult)result;
//            Assert.AreEqual(400, badRequestResult.StatusCode);
//        }

//        [TestMethod]
//        public async Task AddAppointment_ReturnsBadRequest_WhenAppointmentHasFutureDateBeyondLimit()
//        {
//            // Arrange: Invalid appointment with a date more than 30 days in the future
//            var futureDateAppointment = new AppointmentModel
//            {
//                PurposeOfVisit = "Consultation",
//                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(31)), // Invalid Date (Beyond 30 Days)
//                Time = TimeOnly.FromDateTime(DateTime.Now),
//                Email = "test@example.com",
//                Phone = "1234567890",
//                Status = AppointmentStatus.SCHEDULED,
//                Message = "Looking forward",
//                CreatedBy = "Admin",
//                CreatedDate = DateTime.Now,
//                LastModifiedBy = "Admin",
//                LastModifiedDate = DateTime.Now,
//                PatientId = 1,
//                DoctorId = 1
//            };

//            // Act
//            var result = await controller.AddAppointment(futureDateAppointment);

//            // Assert
//            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
//            var badRequestResult = (BadRequestObjectResult)result;
//            Assert.AreEqual(400, badRequestResult.StatusCode);
//        }
//    }

//}