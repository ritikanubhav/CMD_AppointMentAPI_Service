using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public AppointmentManager(IAppointmentRepo appointmentRepo) 
        { 
            this.appointmentRepo=appointmentRepo;
        }

        public async Task CancelAppointment(int id)
        {
            var appointment= await appointmentRepo.GetAppointmentById(id);
            if(appointment.Status == AppointmentStatus.CANCELLED)
            {
                throw new Exception("Already Cancelled Before");
            }
            await appointmentRepo.CancelAppointment(id);
        }

        public async Task CreateAppointment(AppointmentModel appointment)
        {
            if(!DateValidator.ValidateDate(appointment.Date))
            {
                throw new InvalidDataException("Not valid date");
            }
            await appointmentRepo.AddAppointment(appointment);
        }

        public async Task<List<AppointmentModel>> FilterAppointmentsByDate(DateOnly date, int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber,pageSize))
            {
                throw new NotValidPaginationException("Page Size and Page Limit not Valid");
            }
            if(!DateValidator.ValidateDate(date))
            {
                throw new NotValidDateException("Not valid date");
            }
            var filteredAppointments = await appointmentRepo.FilterAppointmentsByDate(date, pageNumber, pageSize);
            return filteredAppointments;
        }

        public async Task<List<AppointmentModel>> FilterAppointmentsByStatus(string status, int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                throw new NotValidPaginationException("Page Size and Page Limit not Valid");
            }
            var filteredAppointments = await appointmentRepo.FilterAppointmentsByStatus(status, pageNumber, pageSize);
            return filteredAppointments;
        }

        public async Task<List<AppointmentModel>> GetActiveAppointments(int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                throw new NotValidPaginationException("Page Size and Page Limit not Valid");
            }
            var filteredAppointments = await appointmentRepo.GetActiveAppointments(pageNumber, pageSize);
            return filteredAppointments;
        }

        public async Task<List<AppointmentModel>> GetAllAppointments(int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                throw new NotValidPaginationException("Page Size and Page Limit not Valid");
            }
            var filteredAppointments = await appointmentRepo.GetAllAppointments(pageNumber, pageSize);
            return filteredAppointments;
        }

        public async Task<List<AppointmentModel>> GetAllAppointmentsByDoctorID(int doctorId, int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                throw new NotValidPaginationException("Page Size and Page Limit not Valid");
            }
            var filteredAppointments = await appointmentRepo.GetAllAppointmentsByDoctorID(doctorId,pageNumber, pageSize);
            return filteredAppointments;
        }

        public async Task<List<AppointmentModel>> GetAllAppointmentsByPatientID(int patientId, int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                throw new NotValidPaginationException("Page Size and Page Limit not Valid");
            }
            var filteredAppointments = await appointmentRepo.GetAllAppointmentsByPatientID(patientId, pageNumber, pageSize);
            return filteredAppointments;
        }

        public Task<AppointmentModel> GetAppointmentById(int id)
        {
            var appointment=appointmentRepo.GetAppointmentById(id);
            if(appointment == null)
            {
                throw new NotFoundException("No Appointment found");
            }
            return appointment;
        }

        public async Task<List<AppointmentModel>> GetInactiveAppointments(int pageNumber, int pageSize)
        {
            if (!PaginationValidator.ValidatePagination(pageNumber, pageSize))
            {
                throw new NotValidPaginationException("Page Size and Page Limit not Valid");
            }
            var filteredAppointments = await appointmentRepo.GetInactiveAppointments(pageNumber, pageSize);
            return filteredAppointments;
        }

        public async Task UpdateAppointment(AppointmentModel appointmentData,int id)
        {
            var appointment=appointmentRepo.GetAppointmentById(id);
            if(appointment==null ||appointment.Id!=id)
            {
                throw new NotFoundException("Not such Appointment");
            }
            if (DateValidator.ValidateDate(appointmentData.Date))
            {
                await appointmentRepo.UpdateAppointment(appointmentData);
            }
            else
            {
                throw new InvalidDataException("Not valid date");
            }
        }
    }
}
