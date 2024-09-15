using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Exceptions;
using CMD.Appointment.Domain.Services;
using Azure;

namespace CMD.Appointment.Data
{
    /// <summary>
    /// Repository class for managing appointment data using Entity Framework Core.
    /// Implements the <see cref="IAppointmentRepo"/> interface.
    /// </summary>
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly AppointmentDbContext db;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentRepo"/> class.
        /// </summary>
        /// <param name="db">The <see cref="AppointmentDbContext"/> to be used by the repository.</param>
        public AppointmentRepo(AppointmentDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Adds a new appointment to the database.
        /// </summary>
        /// <param name="appointmentModel">The appointment to be added.</param>
        public async Task AddAppointment(AppointmentModel appointmentModel)
        {
            await db.Appointments.AddAsync(appointmentModel);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Cancels an existing appointment by setting its status to <see cref="AppointmentStatus.CANCELLED"/>.
        /// </summary>
        /// <param name="id">The ID of the appointment to be cancelled.</param>
        public async Task CancelAppointment(int id)
        {
            var exitingRecord = await db.Appointments.FindAsync(id);
            if (exitingRecord != null)
            {
                exitingRecord.Status = AppointmentStatus.CANCELLED;
                exitingRecord.LastModifiedDate = DateTime.UtcNow;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieves an appointment by its ID.
        /// </summary>
        /// <param name="id">The ID of the appointment to retrieve.</param>
        /// <returns>The appointment with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown when no appointment is found with the specified ID.</exception>
        public async Task<AppointmentModel> GetAppointmentById(int id)
        {
            var appointment = await db.Appointments.FindAsync(id);
            if (appointment != null)
                return appointment;
            else
                throw new NotFoundException("No Appointment Found");
        }

        /// <summary>
        /// Filters appointments by date with pagination.
        /// </summary>
        /// <param name="date">The date to filter appointments by.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A list of appointments for the specified date and page.</returns>
        public async Task<List<AppointmentModel>> FilterAppointmentsByDate(DateOnly date, int pageNumber, int pageSize)
        {
            return await db.Appointments
                .Where(a => a.Date == date)
                .Skip((pageNumber - 1) * pageSize) // Skip records for previous pages
                .Take(pageSize) // Take records for the current page
                .ToListAsync();
        }

        /// <summary>
        /// Filters appointments by status with pagination.
        /// </summary>
        /// <param name="status">The status to filter appointments by.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A list of appointments for the specified status and page.</returns>
        /// <exception cref="ArgumentException">Thrown when the status is not valid.</exception>
        public async Task<List<AppointmentModel>> FilterAppointmentsByStatus(string status, int pageNumber, int pageSize)
        {
            if (!Enum.TryParse(typeof(AppointmentStatus), status, true, out object parsedStatus))
            {
                throw new ArgumentException($"{status} is not valid");
            }

            var statusEnum = (AppointmentStatus)parsedStatus;

            return await db.Appointments
                .Where(a => a.Status == statusEnum)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a list of active appointments with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A list of active appointments for the specified page.</returns>
        public async Task<List<AppointmentModel>> GetActiveAppointments(int pageNumber = 1, int pageSize = 10)
        {
            return await db.Appointments
                .Where(a => a.Status == AppointmentStatus.SCHEDULED)
                .Skip((pageNumber - 1) * pageSize) // Skips the records for previous pages
                .Take(pageSize) // Takes the records for the current page
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a list of inactive appointments (cancelled or closed) with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A list of inactive appointments for the specified page.</returns>
        public async Task<List<AppointmentModel>> GetInactiveAppointments(int pageNumber, int pageSize)
        {
            return await db.Appointments
                .Where(a => a.Status == AppointmentStatus.CANCELLED || a.Status == AppointmentStatus.CLOSED)
                .Skip((pageNumber - 1) * pageSize) // Skips the records for previous pages
                .Take(pageSize) // Takes the records for the current page
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all appointments with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>An <see cref="AppointmentResponse"/> containing the appointments and pagination details.</returns>
        public async Task<AppointmentResponse> GetAllAppointments(int pageNumber = 1, int pageSize = 10)
        {
            var totalNoOfAppointments = await db.Appointments.CountAsync();
            var pagedAppointments = await db.Appointments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new AppointmentResponse()
            {
                Items = pagedAppointments,
                PageLimit = pageSize,
                PageNumber = pageNumber,
                TotalAppointments = totalNoOfAppointments
            };
        }

        /// <summary>
        /// Updates an existing appointment.
        /// </summary>
        /// <param name="appointmentModel">The appointment to be updated.</param>
        public async Task UpdateAppointment(AppointmentModel appointmentModel)
        {
            db.Appointments.Update(appointmentModel);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all appointments for a specific patient ID with pagination.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A list of appointments for the specified patient ID and page.</returns>
        public async Task<List<AppointmentModel>> GetAllAppointmentsByPatientID(int patientId, int pageNumber, int pageSize)
        {
            return await db.Appointments
                .Where(a => a.PatientId == patientId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all appointments for a specific doctor ID with pagination.
        /// </summary>
        /// <param name="doctorId">The ID of the doctor.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A list of appointments for the specified doctor ID and page.</returns>
        public async Task<List<AppointmentModel>> GetAllAppointmentsByDoctorID(int doctorId, int pageNumber, int pageSize)
        {
            return await db.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
