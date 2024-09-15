using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Appointment.Domain.DTO;
using CMD.Appointment.Domain.Entities;

namespace CMD.Appointment.Domain.Manager
{
    /// <summary>
    /// Defines the operations for managing appointments, including creating, updating, and retrieving appointment data.
    /// </summary>
    public interface IAppointmentManager
    {
        /// <summary>
        /// Creates a new appointment.
        /// </summary>
        /// <param name="appointment">The appointment model containing appointment details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task CreateAppointment(AppointmentModel appointment);

        /// <summary>
        /// Updates an existing appointment based on the provided data.
        /// </summary>
        /// <param name="appointment">The updated appointment data.</param>
        /// <param name="id">The ID of the appointment to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task UpdateAppointment(UpdateAppointmentDTO appointment, int id);

        /// <summary>
        /// Retrieves an appointment by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the appointment to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, with a result containing the appointment model.</returns>
        public Task<AppointmentModel> GetAppointmentById(int id);

        /// <summary>
        /// Retrieves all appointments with pagination support.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation, with a result containing a response with appointment data.</returns>
        public Task<AppointmentResponse> GetAllAppointments(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves all active appointments with pagination support.
        /// </summary>
        /// <param name="pagenumber">The page number to retrieve.</param>
        /// <param name="pagesize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation, with a result containing a list of active appointment models.</returns>
        public Task<List<AppointmentModel>> GetActiveAppointments(int pagenumber, int pagesize);

        /// <summary>
        /// Retrieves all inactive appointments with pagination support.
        /// </summary>
        /// <param name="pagenumber">The page number to retrieve.</param>
        /// <param name="pagesize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation, with a result containing a list of inactive appointment models.</returns>
        public Task<List<AppointmentModel>> GetInactiveAppointments(int pagenumber, int pagesize);

        /// <summary>
        /// Filters appointments based on the specified date and provides results with pagination support.
        /// </summary>
        /// <param name="date">The date to filter appointments by.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation, with a result containing a list of filtered appointment models.</returns>
        public Task<List<AppointmentModel>> FilterAppointmentsByDate(DateOnly date, int pageNumber, int pageSize);

        /// <summary>
        /// Filters appointments based on the specified status and provides results with pagination support.
        /// </summary>
        /// <param name="status">The status to filter appointments by.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation, with a result containing a list of filtered appointment models.</returns>
        public Task<List<AppointmentModel>> FilterAppointmentsByStatus(string status, int pageNumber, int pageSize);

        /// <summary>
        /// Cancels an appointment based on its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the appointment to cancel.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task CancelAppointment(int id);

        /// <summary>
        /// Retrieves all appointments for a specific patient with pagination support.
        /// </summary>
        /// <param name="patientId">The ID of the patient whose appointments are to be retrieved.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation, with a result containing a list of appointment models for the specified patient.</returns>
        public Task<List<AppointmentModel>> GetAllAppointmentsByPatientID(int patientId, int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves all appointments for a specific doctor with pagination support.
        /// </summary>
        /// <param name="doctorId">The ID of the doctor whose appointments are to be retrieved.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation, with a result containing a list of appointment models for the specified doctor.</returns>
        public Task<List<AppointmentModel>> GetAllAppointmentsByDoctorID(int doctorId, int pageNumber, int pageSize);
    }
}
