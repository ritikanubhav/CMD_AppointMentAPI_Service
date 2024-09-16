using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CMD.Appointment.Domain.DTO;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.Exceptions;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NLog;

namespace CMD.Appointment.Domain.Manager
{
    /// <summary>
    /// Manages operations related to appointments, including creation, retrieval, and cancellation.
    /// </summary>
    public class AppointmentManager : IAppointmentManager
    {

        private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IAppointmentRepo appointmentRepo;
        private readonly IMessageService messageService;

        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentManager"/> class.
        /// </summary>
        /// <param name="appointmentRepo">The repository for appointment data operations.</param>
        /// <param name="mapper">The mapper for mapping between entities and DTOs.</param>
        /// <param name="messageService">The service for retrieving localized messages.</param>
        public AppointmentManager(IAppointmentRepo appointmentRepo, IMapper mapper, IMessageService messageService)
        {
            this.appointmentRepo = appointmentRepo;
            this.messageService = messageService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Cancels an appointment by its ID.
        /// </summary>
        /// <param name="appointmentId">The ID of the appointment to cancel.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown when the appointment does not exist.</exception>
        /// <exception cref="BusinessException">Thrown when the appointment status is not SCHEDULED.</exception>
        /// <exception cref="AppointmentUnableToCancelException">Thrown if the cancellation is requested within 24 hours of the appointment.</exception>
        public async Task CancelAppointment(int appointmentId)
        {
            _logger.Info($"CancelAppointment started for appointmentId: {appointmentId}");

            var appointment = await appointmentRepo.GetAppointmentById(appointmentId);
            if (appointment == null)
            {

                _logger.Warn($"No appointment found with appointmentId: {appointmentId}");
                throw new Exception($"There is no appointment witht the appointmentId {appointmentId}");
            }

            //if the record exists and its status should be SCHEDULED
            if (appointment != null)
            {
                if (appointment.Status != AppointmentStatus.SCHEDULED)
                {
                    _logger.Warn($"Appointment with appointmentId: {appointmentId} is not in SCHEDULED status");
                    throw new BusinessException(messageService.GetMessage("CompletedAppointmentCancellation"));
                }
                if ((appointment.Date.ToDateTime(appointment.Time) - DateTime.Now).TotalHours < 24)
                {
                    _logger.Warn($"Appointment with appointmentId: {appointmentId} cannot be cancelled within 24 hours");
                    throw new AppointmentUnableToCancelException(messageService.GetMessage("TwentyFourHoursPolicy"));
                }

                await appointmentRepo.CancelAppointment(appointmentId);
                _logger.Info($"Appointment with appointmentId: {appointmentId} cancelled successfully");
                throw new Exception($"There is no appointment with the appointmentId {appointmentId}");
            }

            // If the record exists and its status should be SCHEDULED
            if (appointment.Status != AppointmentStatus.SCHEDULED)
            {
                throw new BusinessException(messageService.GetMessage("CompletedAppointmentCancellation"));
            }
            if ((appointment.Date.ToDateTime(appointment.Time) - DateTime.Now).TotalHours < 24)
            {
                throw new AppointmentUnableToCancelException(messageService.GetMessage("TwentyFourHoursPolicy"));
            }

            await appointmentRepo.CancelAppointment(appointmentId);
        }

        /// <summary>
        /// Creates a new appointment.
        /// </summary>
        /// <param name="appointment">The appointment model to create.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidDataException">Thrown when the appointment date is invalid.</exception>
        /// <exception cref="InvalidPatientIdException">Thrown when the patient ID is invalid.</exception>
        /// <exception cref="InvalidDoctorIdException">Thrown when the doctor ID is invalid.</exception>
        public async Task CreateAppointment(AppointmentModel appointment)
        {
            _logger.Info($"CreateAppointment started for patientId: {appointment.PatientId} with doctorId: {appointment.DoctorId}");

            if (!DateValidator.ValidateDate(appointment.Date))
            {
                _logger.Warn($"Invalid appointment date: {appointment.Date}");
                throw new InvalidDataException(messageService.GetMessage("InvalidAppointment"));
            }

            if (!await PatientValidator.ValidatePatientIdAsync(appointment.PatientId))
            {
                _logger.Warn($"Invalid patientId: {appointment.PatientId}");
                throw new InvalidPatientIdException(messageService.GetMessage("InvalidPatientId"));
            }

            if (!await DoctorIdValidator.ValidateDoctorIdAsync(appointment.DoctorId))
            {
                _logger.Warn($"Invalid doctorId: {appointment.DoctorId}");
                throw new InvalidDoctorIdException(messageService.GetMessage("InvalidDoctorId"));
            }

            await appointmentRepo.AddAppointment(appointment);
            _logger.Info($"Appointment created successfully for patientId: {appointment.PatientId} and doctorId: {appointment.DoctorId}");
        }

        /// <summary>
        /// Filters appointments by date with pagination.
        /// </summary>
        /// <param name="date">The date to filter appointments by.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of filtered appointment models.</returns>
        /// <exception cref="NotValidPaginationException">Thrown when the pagination parameters are invalid.</exception>
        /// <exception cref="NotValidDateException">Thrown when the provided date is not valid.</exception>
        public async Task<List<AppointmentModel>> FilterAppointmentsByDate(DateOnly date, int pageNumber, int pageSize)
        {

            _logger.Info($"FilterAppointmentsByDate started for date: {date}, pageNumber: {pageNumber}, pageSize: {pageSize}");


            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                _logger.Warn($"Invalid pagination parameters: pageNumber: {pageNumber}, pageSize: {pageSize}");
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }

            if (!DateValidator.ValidateDate(date))
            {
                _logger.Warn($"Invalid date: {date}");
                throw new NotValidDateException(messageService.GetMessage("InvalidDate"));
            }

            var filteredAppointments = await appointmentRepo.FilterAppointmentsByDate(date, pageNumber, pageSize);
            _logger.Info($"Filtered {filteredAppointments.Count} appointments by date: {date}");
            return filteredAppointments;
        }

        /// <summary>
        /// Filters appointments by status with pagination.
        /// </summary>
        /// <param name="status">The status to filter appointments by.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of filtered appointment models.</returns>
        /// <exception cref="NotValidPaginationException">Thrown when the pagination parameters are invalid.</exception>
        public async Task<List<AppointmentModel>> FilterAppointmentsByStatus(string status, int pageNumber, int pageSize)
        {
            _logger.Info($"FilterAppointmentsByStatus started for status: {status}, pageNumber: {pageNumber}, pageSize: {pageSize}");

            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                _logger.Warn($"Invalid pagination parameters: pageNumber: {pageNumber}, pageSize: {pageSize}");
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }

            var filteredAppointments = await appointmentRepo.FilterAppointmentsByStatus(status, pageNumber, pageSize);
            _logger.Info($"Filtered {filteredAppointments.Count} appointments by status: {status}");
            return filteredAppointments;
        }

        /// <summary>
        /// Retrieves active appointments with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of active appointment models.</returns>
        /// <exception cref="NotValidPaginationException">Thrown when the pagination parameters are invalid.</exception>
        public async Task<List<AppointmentModel>> GetActiveAppointments(int pageNumber, int pageSize)
        {
            _logger.Info($"GetActiveAppointments started with pageNumber: {pageNumber}, pageSize: {pageSize}");

            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                _logger.Warn($"Invalid pagination parameters: pageNumber: {pageNumber}, pageSize: {pageSize}");
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }

            var filteredAppointments = await appointmentRepo.GetActiveAppointments(pageNumber, pageSize);
            _logger.Info($"Retrieved {filteredAppointments.Count} active appointments");
            return filteredAppointments;
        }

        /// <summary>
        /// Retrieves a list of inactive appointments with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of inactive appointment models.</returns>
        /// <exception cref="NotValidPaginationException">Thrown when the pagination parameters are invalid.</exception>
       public async Task<List<AppointmentModel>> GetInactiveAppointments(int pageNumber, int pageSize)
        {
            _logger.Info($"GetInactiveAppointments started with pageNumber: {pageNumber}, pageSize: {pageSize}");

            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                _logger.Warn($"Invalid pagination parameters: pageNumber: {pageNumber}, pageSize: {pageSize}");
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }

            var filteredAppointments = await appointmentRepo.GetInactiveAppointments(pageNumber, pageSize);
            _logger.Info($"Retrieved {filteredAppointments.Count} inactive appointments");
            return filteredAppointments;
        }

        /// <summary>
        /// Retrieves all appointments with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains all appointments with pagination information.</returns>
        /// <exception cref="NotValidPaginationException">Thrown when the pagination parameters are invalid.</exception>
        public async Task<AppointmentResponse> GetAllAppointments(int pageNumber, int pageSize)
        {
            _logger.Info($"GetAllAppointments started with pageNumber: {pageNumber}, pageSize: {pageSize}");

            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                _logger.Warn($"Invalid pagination parameters: pageNumber: {pageNumber}, pageSize: {pageSize}");
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }

            var filteredAppointments = await appointmentRepo.GetAllAppointments(pageNumber, pageSize);
            return filteredAppointments;
        }

        /// <summary>
        /// Retrieves all appointments for a specific doctor with pagination.
        /// </summary>
        /// <param name="doctorId">The ID of the doctor whose appointments to retrieve.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of appointment models for the specified doctor.</returns>
        /// <exception cref="NotValidPaginationException">Thrown when the pagination parameters are invalid.</exception>
        public async Task<List<AppointmentModel>> GetAllAppointmentsByDoctorID(int doctorId, int pageNumber, int pageSize)
        {
            _logger.Info($"GetAllAppointmentsByDoctorID started for doctorId: {doctorId}, pageNumber: {pageNumber}, pageSize: {pageSize}");

            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                _logger.Warn($"Invalid pagination parameters: pageNumber: {pageNumber}, pageSize: {pageSize}");
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }


            var filteredAppointments = await appointmentRepo.GetAllAppointmentsByDoctorID(doctorId, pageNumber, pageSize);
            _logger.Info($"Retrieved {filteredAppointments.Count} appointments for doctorId: {doctorId}");

            return filteredAppointments;
        }

        /// <summary>
        /// Retrieves all appointments for a specific patient with pagination.
        /// </summary>
        /// <param name="patientId">The ID of the patient whose appointments to retrieve.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of appointments per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of appointment models for the specified patient.</returns>
        /// <exception cref="NotValidPaginationException">Thrown when the pagination parameters are invalid.</exception>
        public async Task<List<AppointmentModel>> GetAllAppointmentsByPatientID(int patientId, int pageNumber, int pageSize)
        {
            _logger.Info($"GetAllAppointmentsByPatientID started for patientId: {patientId}, pageNumber: {pageNumber}, pageSize: {pageSize}");

            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                _logger.Warn($"Invalid pagination parameters: pageNumber: {pageNumber}, pageSize: {pageSize}");
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }

            var filteredAppointments = await appointmentRepo.GetAllAppointmentsByPatientID(patientId, pageNumber, pageSize);
            _logger.Info($"Retrieved {filteredAppointments.Count} appointments for patientId: {patientId}");
            return filteredAppointments;
        }


        
        /// <summary>
        /// Retrieves an appointment by its ID.
        /// </summary>
        /// <param name="id">The ID of the appointment to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the appointment model.</returns>
        /// <exception cref="NotFoundException">Thrown when the appointment does not exist.</exception>
        public async Task<AppointmentModel> GetAppointmentById(int id)
        {
        _logger.Info($"GetAppointmentById started for id: {id}");
            var appointment = appointmentRepo.GetAppointmentById(id);

            if (appointment == null)
            {
                _logger.Warn($"No appointment found with id: {id}");
                throw new NotFoundException(messageService.GetMessage("InvalidAppointment"));
            }

            _logger.Info($"Retrieved appointment with id: {id}");
            return appointment;
        }


        


        /// <summary>
        /// Updates an existing appointment.
        /// </summary>
        /// <param name="appointmentData">The updated appointment data.</param>
        /// <param name="id">The ID of the appointment to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown when the appointment does not exist.</exception>
        /// <exception cref="InvalidDataException">Thrown when the updated appointment date is invalid.</exception>
        public async Task UpdateAppointment(UpdateAppointmentDTO appointmentData, int id)
        {
            _logger.Info($"UpdateAppointment started for id: {id}");

            // Check if the appointment exists
            var appointment = await appointmentRepo.GetAppointmentById(id);
            if (appointment == null)
            {
                _logger.Warn($"No appointment found with id: {id}");
                throw new NotFoundException(messageService.GetMessage("InvalidAppointment"));
            }

            if (!DateValidator.ValidateDate(appointmentData.Date))
            {
                _logger.Warn($"Invalid date for update: {appointmentData.Date}");
                throw new InvalidDataException("Not valid date");
            }
            else
            {
                mapper.Map(appointmentData, appointment);
                await appointmentRepo.UpdateAppointment(appointment);
                _logger.Info($"Appointment with id: {id} updated successfully");
            }
        }
    }
}
