using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.Exceptions;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CMD.Appointment.Domain.Manager
{
    public class AppointmentManager : IAppointmentManager
    {
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
            var appointment = await appointmentRepo.GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                throw new Exception($"There is no appointment witht the appointmentId {appointmentId}");
            }

            //if the record exists and its status should be SCHEDULED
            if (appointment != null)
            {
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
        }

        public async Task CreateAppointment(AppointmentModel appointment)
        {
            if(!DateValidator.ValidateDate(appointment.Date))
            {
                throw new InvalidDataException(messageService.GetMessage("InvalidAppointment"));
            }
            if(!await PatientValidator.ValidatePatientIdAsync(appointment.PatientId))
            {
                throw new InvalidPatientIdException(messageService.GetMessage("InvalidPatientId"));
            }
            await appointmentRepo.AddAppointment(appointment);
        }

        public async Task<List<AppointmentModel>> FilterAppointmentsByDate(DateOnly date, int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber,pageSize))
            {
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }
            if(!DateValidator.ValidateDate(date))
            {
                throw new NotValidDateException(messageService.GetMessage("InvalidDate"));
            }
            var filteredAppointments = await appointmentRepo.FilterAppointmentsByDate(date, pageNumber, pageSize);
            return filteredAppointments;
        }

        public async Task<List<AppointmentModel>> FilterAppointmentsByStatus(string status, int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }
            var filteredAppointments = await appointmentRepo.FilterAppointmentsByStatus(status, pageNumber, pageSize);
            return filteredAppointments;
        }

        public async Task<List<AppointmentModel>> GetActiveAppointments(int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }
            var filteredAppointments = await appointmentRepo.GetActiveAppointments(pageNumber, pageSize);
            return filteredAppointments;
        }

        public async Task<List<AppointmentModel>> GetAllAppointments(int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }
            var filteredAppointments = await appointmentRepo.GetAllAppointments(pageNumber, pageSize);
            return filteredAppointments;
        }

        public async Task<List<AppointmentModel>> GetAllAppointmentsByDoctorID(int doctorId, int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }
            var filteredAppointments = await appointmentRepo.GetAllAppointmentsByDoctorID(doctorId,pageNumber, pageSize);
            return filteredAppointments;
        }

        public async Task<List<AppointmentModel>> GetAllAppointmentsByPatientID(int patientId, int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }
            var filteredAppointments = await appointmentRepo.GetAllAppointmentsByPatientID(patientId, pageNumber, pageSize);
            return filteredAppointments;
        }

        public Task<AppointmentModel> GetAppointmentById(int id)
        {
            var appointment=appointmentRepo.GetAppointmentById(id);
            if(appointment == null)
            {
                throw new NotFoundException(messageService.GetMessage("InvalidAppointment"));
            }
            return appointment;
        }

        public async Task<List<AppointmentModel>> GetInactiveAppointments(int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                throw new NotValidPaginationException(messageService.GetMessage("InvalidPagination"));
            }
            var filteredAppointments = await appointmentRepo.GetInactiveAppointments(pageNumber, pageSize);
            return filteredAppointments;
        }

        public async Task UpdateAppointment(AppointmentModel appointmentData,int id)
        {
            // Check if the appointment exists
            var appointment = await appointmentRepo.GetAppointmentById(id);
            if (appointment == null)
            {
                throw new NotFoundException(messageService.GetMessage("InvalidAppointment"));
            }
            if (!DateValidator.ValidateDate(appointmentData.Date))
            {
                throw new InvalidDataException("Not valid date");
            }
            if (!await PatientValidator.ValidatePatientIdAsync(appointment.PatientId))
            {
                throw new InvalidPatientIdException(messageService.GetMessage("InvalidPatientId"));
            }
            else
            {
                mapper.Map(appointmentData,appointment);
                await appointmentRepo.UpdateAppointment(appointment);
            }
        }
    }
}
