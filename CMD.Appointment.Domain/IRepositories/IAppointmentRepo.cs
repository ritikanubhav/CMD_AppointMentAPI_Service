using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMD.Appointment.Domain.Entities;

namespace CMD.Appointment.Domain.IRepositories
{
    /// <summary>
    /// Defines the contract for appointment repository operations.
    /// </summary>
    public interface IAppointmentRepo
    {
        /// <summary>
        /// Adds a new appointment to the repository.
        /// </summary>
        /// <param name="appointmentModel">The appointment model to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task AddAppointment(AppointmentModel appointmentModel);

        /// <summary>
        /// Updates an existing appointment in the repository.
        /// </summary>
        /// <param name="appointmentModel">The appointment model to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task UpdateAppointment(AppointmentModel appointmentModel);

        /// <summary>
        /// Retrieves an appointment by its ID.
        /// </summary>
        /// <param name="id">The ID of the appointment to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the appointment model.</returns>
        public Task<AppointmentModel> GetAppointmentById(int id);

        /// <summary>
        /// Retrieves all appointments with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of appointment models and pagination information.</returns>
        public Task<AppointmentResponse> GetAllAppointments(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves active appointments with pagination.
        /// </summary>
        /// <param name="pagenumber">The page number to retrieve.</param>
        /// <param name="pagesize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of active appointment models.</returns>
        public Task<List<AppointmentModel>> GetActiveAppointments(int pagenumber, int pagesize);

        /// <summary>
        /// Retrieves inactive appointments with pagination.
        /// </summary>
        /// <param name="pagenumber">The page number to retrieve.</param>
        /// <param name="pagesize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of inactive appointment models.</returns>
        public Task<List<AppointmentModel>> GetInactiveAppointments(int pagenumber, int pagesize);

        /// <summary>
        /// Filters appointments by date with pagination.
        /// </summary>
        /// <param name="date">The date to filter appointments by.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of appointment models that match the specified date.</returns>
        public Task<List<AppointmentModel>> FilterAppointmentsByDate(DateOnly date, int pageNumber, int pageSize);

        /// <summary>
        /// Filters appointments by status with pagination.
        /// </summary>
        /// <param name="status">The status to filter appointments by.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of appointment models that match the specified status.</returns>
        public Task<List<AppointmentModel>> FilterAppointmentsByStatus(string status, int pageNumber, int pageSize);

        /// <summary>
        /// Cancels an appointment by its ID.
        /// </summary>
        /// <param name="id">The ID of the appointment to cancel.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task CancelAppointment(int id);

        /// <summary>
        /// Retrieves all appointments for a specific patient with pagination.
        /// </summary>
        /// <param name="patientId">The ID of the patient whose appointments to retrieve.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of appointment models for the specified patient.</returns>
        public Task<List<AppointmentModel>> GetAllAppointmentsByPatientID(int patientId, int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves all appointments for a specific doctor with pagination.
        /// </summary>
        /// <param name="doctorId">The ID of the doctor whose appointments to retrieve.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of appointment models for the specified doctor.</returns>
        public Task<List<AppointmentModel>> GetAllAppointmentsByDoctorID(int doctorId, int pageNumber, int pageSize);
    }
}
