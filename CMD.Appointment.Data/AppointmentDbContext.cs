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
                    PurposeOfVisit = "General Checkup",
                    Date = new DateOnly(2024, 9, 20),
                    Time = new TimeOnly(9, 30), // 9:30 AM
                    Email = "john.doe@example.com",
                    Phone = "123-456-7890",
                    Status = AppointmentStatus.SCHEDULED,
                    Message = "Please bring your medical reports.",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = "Admin",
                    LastModifiedDate = DateTime.Now,
                    PatientId = 101,
                    DoctorId = 202
                },
                new AppointmentModel
                {
                    Id = 2,
                    PurposeOfVisit = "Follow-up on blood test",
                    Date = new DateOnly(2024, 9, 22),
                    Time = new TimeOnly(11, 0), // 11:00 AM
                    Email = "jane.smith@example.com",
                    Phone = "987-654-3210",
                    Status = AppointmentStatus.CANCELLED,
                    Message = "Appointment canceled by patient.",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = "Admin",
                    LastModifiedDate = DateTime.Now,
                    PatientId = 102,
                    DoctorId = 203
                },
                new AppointmentModel
                {
                    Id = 3,
                    PurposeOfVisit = "Dental Checkup",
                    Date = new DateOnly(2024, 9, 25),
                    Time = new TimeOnly(10, 0), // 10:00 AM
                    Email = "alice.jones@example.com",
                    Phone = "555-123-4567",
                    Status = AppointmentStatus.CLOSED,
                    Message = "Dental checkup completed.",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = "Admin",
                    LastModifiedDate = DateTime.Now,
                    PatientId = 103,
                    DoctorId = 204
                },
                new AppointmentModel
                {
                    Id = 4,
                    PurposeOfVisit = "Eye Examination",
                    Date = new DateOnly(2024, 9, 26),
                    Time = new TimeOnly(14, 0), // 2:00 PM
                    Email = "mike.brown@example.com",
                    Phone = "555-987-6543",
                    Status = AppointmentStatus.SCHEDULED,
                    Message = "Please bring your previous prescription.",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = "Admin",
                    LastModifiedDate = DateTime.Now,
                    PatientId = 104,
                    DoctorId = 205
                },
                new AppointmentModel
                {
                    Id = 5,
                    PurposeOfVisit = "Orthopedic Consultation",
                    Date = new DateOnly(2024, 9, 28),
                    Time = new TimeOnly(16, 30), // 4:30 PM
                    Email = "chris.evans@example.com",
                    Phone = "555-654-7891",
                    Status = AppointmentStatus.CANCELLED,
                    Message = "Doctor is unavailable, appointment rescheduled.",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = "Admin",
                    LastModifiedDate = DateTime.Now,
                    PatientId = 105,
                    DoctorId = 206
                }
            );
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{AppointmentModel}"/> for appointments.
        /// </summary>
        public DbSet<AppointmentModel> Appointments { get; set; }
    }
}
