using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CMD.Appointment.Data
{
    /// <summary>
    /// Entity Framework Core DbContext for managing appointment data.
    /// </summary>
    public class AppointmentDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentDbContext"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentDbContext"/> class.
        /// </summary>
        public AppointmentDbContext()
        { }

        public void ClearDatabase()
        {
            // Ensure that foreign key constraints are handled correctly
            Database.ExecuteSqlRaw("DELETE FROM Appointments");
        }

        /// <summary>
        /// Configures the model using the <see cref="ModelBuilder"/> API.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to use for configuring the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppointmentModel>().HasData(
            new AppointmentModel
            {
                Id = 1,
                PurposeOfVisit = "Consultation",
                Date = new DateOnly(2024, 9, 20),
                Time = new TimeOnly(9, 30), // 9:30 AM
                Email = "john.doe@example.com",
                Phone = "919876543210",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Please bring your medical reports.",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 1,
                DoctorId = 1
            },
            new AppointmentModel
            {
                Id = 2,
                PurposeOfVisit = "Follow Up",
                Date = new DateOnly(2024, 9, 22),
                Time = new TimeOnly(11, 0), // 11:00 AM
                Email = "jane.smith@example.com",
                Phone = "919234567890",
                Status = AppointmentStatus.CANCELLED,
                Message = "Appointment canceled by patient.",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 2,
                DoctorId = 2
            },
            new AppointmentModel
            {
                Id = 3,
                PurposeOfVisit = "Treatment",
                Date = new DateOnly(2024, 9, 25),
                Time = new TimeOnly(10, 0), // 10:00 AM
                Email = "alice.jones@example.com",
                Phone = "919345678901",
                Status = AppointmentStatus.CLOSED,
                Message = "Dental checkup completed.",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 3,
                DoctorId = 3
            },
            new AppointmentModel
            {
                Id = 4,
                PurposeOfVisit = "Emergency",
                Date = new DateOnly(2024, 9, 26),
                Time = new TimeOnly(14, 0), // 2:00 PM
                Email = "mike.brown@example.com",
                Phone = "919456789012",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Immediate attention required.",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 4,
                DoctorId = 4
            },
            new AppointmentModel
            {
                Id = 5,
                PurposeOfVisit = "General Checkup",
                Date = new DateOnly(2024, 9, 28),
                Time = new TimeOnly(16, 30), // 4:30 PM
                Email = "chris.evans@example.com",
                Phone = "919567890123",
                Status = AppointmentStatus.CANCELLED,
                Message = "Doctor is unavailable, appointment rescheduled.",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 5,
                DoctorId = 5
            },
            new AppointmentModel
            {
                Id = 6,
                PurposeOfVisit = "Typhoid",
                Date = new DateOnly(2024, 9, 30),
                Time = new TimeOnly(8, 0), // 8:00 AM
                Email = "emma.williams@example.com",
                Phone = "919678901234",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Please bring your test reports.",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 1,
                DoctorId = 2
            },
            new AppointmentModel
            {
                Id = 7,
                PurposeOfVisit = "Malaria",
                Date = new DateOnly(2024, 10, 1),
                Time = new TimeOnly(15, 0), // 3:00 PM
                Email = "olivia.johnson@example.com",
                Phone = "919789012345",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Bring blood test results.",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 2,
                DoctorId = 3
            },
            new AppointmentModel
            {
                Id = 8,
                PurposeOfVisit = "Cold",
                Date = new DateOnly(2024, 10, 3),
                Time = new TimeOnly(13, 0), // 1:00 PM
                Email = "ava.miller@example.com",
                Phone = "919890123456",
                Status = AppointmentStatus.CANCELLED,
                Message = "Patient requested cancellation.",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 3,
                DoctorId = 4
            },
            new AppointmentModel
            {
                Id = 9,
                PurposeOfVisit = "Fever",
                Date = new DateOnly(2024, 10, 5),
                Time = new TimeOnly(11, 0), // 11:00 AM
                Email = "isabella.taylor@example.com",
                Phone = "919901234567",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Take medication before visit.",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 4,
                DoctorId = 5
            },
            new AppointmentModel
            {
                Id = 10,
                PurposeOfVisit = "Consultation",
                Date = new DateOnly(2024, 10, 7),
                Time = new TimeOnly(9, 30), // 9:30 AM
                Email = "mia.anderson@example.com",
                Phone = "919012345678",
                Status = AppointmentStatus.SCHEDULED,
                Message = "No special instructions.",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 5,
                DoctorId = 1
            },
            new AppointmentModel
            {
                Id = 11,
                PurposeOfVisit = "Follow Up",
                Date = new DateOnly(2024, 10, 10),
                Time = new TimeOnly(14, 0), // 2:00 PM
                Email = "lucas.martin@example.com",
                Phone = "919123456789",
                Status = AppointmentStatus.CANCELLED,
                Message = "Doctor unavailable, please reschedule.",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 1,
                DoctorId = 3
            },
            new AppointmentModel
            {
                Id = 12,
                PurposeOfVisit = "Treatment",
                Date = new DateOnly(2024, 10, 12),
                Time = new TimeOnly(16, 30), // 4:30 PM
                Email = "sophia.lee@example.com",
                Phone = "919234567890",
                Status = AppointmentStatus.SCHEDULED,
                Message = "Please bring your previous reports.",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "Admin",
                LastModifiedDate = DateTime.Now,
                PatientId = 2,
                DoctorId = 4
            }
            );
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{AppointmentModel}"/> for appointments.
        /// </summary>
        public DbSet<AppointmentModel> Appointments { get; set; }
    }
}
