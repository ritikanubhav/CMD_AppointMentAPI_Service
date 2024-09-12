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


namespace CMD.Appointment.Data
{
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly AppointmentDbContext db;
        public AppointmentRepo(AppointmentDbContext db) 
        {
            this.db = db;
            
        }

        public async Task AddAppointment(AppointmentModel appointmentModel)
        {
            await db.Appointments.AddAsync(appointmentModel);
            await db.SaveChangesAsync();
        }
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
        public async Task<AppointmentModel> GetAppointmentById(int id)
        {
            //only for the appointment ID
            var appointment = await db.Appointments.FindAsync(id);
            if(appointment != null)
                return appointment;
            else 
                throw new NotFoundException("No Appointment Found");
        }

        public async Task<List<AppointmentModel>> FilterAppointmentsByDate(DateOnly date, int pageNumber, int pageSize)
        {
            return await db.Appointments
                .Where(a => a.Date == date)
                .Skip((pageNumber - 1) * pageSize) // Skip records for previous pages
                .Take(pageSize) // Take records for the current page
                .ToListAsync();
        }

        public async Task<List<AppointmentModel>> FilterAppointmentsByStatus(string status, int pageNumber, int pageSize)
        {
            // Parse the string to enum, and EF will match the int values in the database
            var statusEnum = (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), status, true);

            return await db.Appointments
                .Where(a => a.Status == statusEnum)
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<AppointmentModel>> GetActiveAppointments(int pageNumber = 1, int pageSize = 10)
        {
            var pagedAppointments = await db.Appointments
                                            .Where(a => a.Status ==  AppointmentStatus.SCHEDULED)
                                            .Skip((pageNumber - 1) * pageSize) // Skips the records for previous pages
                                            .Take(pageSize) // Takes the records for the current page
                                            .ToListAsync();
            return pagedAppointments;
        }
        public async Task<List<AppointmentModel>> GetInactiveAppointments(int pageNumber, int pageSize)
        {
            var pagedAppointments = await db.Appointments
                .Where(a => a.Status == AppointmentStatus.CANCELLED||a.Status==AppointmentStatus.CLOSED)
                .Skip((pageNumber - 1) * pageSize) // Skips the records for previous pages
                .Take(pageSize) // Takes the records for the current page
                .ToListAsync();
            return pagedAppointments;
        }

        public async Task<List<AppointmentModel>> GetAllAppointments(int pageNumber = 1, int pageSize = 10)
        {
            var pagedAppointments = await db.Appointments
                                         .Skip((pageNumber-1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

            return pagedAppointments;
        }
        public async Task UpdateAppointment(AppointmentModel appointmentModel)
        {
            db.Appointments.Update(appointmentModel);
            await db.SaveChangesAsync();              
        }

        public async Task<List<AppointmentModel>> GetAllAppointmentsByPatientID(int patientId, int pageNumber, int pageSize)
        {
            var pagedAppointments = await db.Appointments.Where(a=>a.PatientId==patientId)
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();
            return pagedAppointments;
        }

        public async Task<List<AppointmentModel>> GetAllAppointmentsByDoctorID(int doctorId, int pageNumber, int pageSize)
        {
            var pagedAppointments = await db.Appointments.Where(a => a.DoctorId == doctorId)
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();
            return pagedAppointments;
        }
    }
}
