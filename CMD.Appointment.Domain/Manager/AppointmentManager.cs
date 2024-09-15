using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class AppointmentManager : IAppointmentManager
    {
        private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        private IAppointmentRepo appointmentRepo;
        private IMessageService messageService;
        private readonly IMapper mapper;
        public AppointmentManager(IAppointmentRepo appointmentRepo,IMapper mapper, IMessageService messageService) 
        { 
            this.appointmentRepo=appointmentRepo;
            this.messageService=messageService;
            this.mapper=mapper;
        }

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
            }
        }

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

        public async Task<AppointmentModel> GetAppointmentById(int id)
        {

            _logger.Info($"GetAppointmentById started for id: {id}");

            var appointment = await appointmentRepo.GetAppointmentById(id);
            if (appointment == null)
            {
                _logger.Warn($"No appointment found with id: {id}");
                throw new NotFoundException(messageService.GetMessage("InvalidAppointment"));
            }

            _logger.Info($"Retrieved appointment with id: {id}");
            return appointment;
        }

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

        public async Task UpdateAppointment(UpdateAppointmentDTO appointmentData,int id)
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
