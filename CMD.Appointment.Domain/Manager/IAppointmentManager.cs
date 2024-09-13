using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Appointment.Domain.DTO;
using CMD.Appointment.Domain.Entities;

namespace CMD.Appointment.Domain.Manager
{
    public  interface IAppointmentManager
    {
        public Task CreateAppointment(AppointmentModel appointment);
        public Task UpdateAppointment(UpdateAppointmentDTO appointment,int id);
        public Task<AppointmentModel> GetAppointmentById(int id);
        public Task<List<AppointmentModel>> GetAllAppointments(int pageNumber, int pageSize);
        public Task<List<AppointmentModel>> GetActiveAppointments(int pagenumber, int pagesize);
        public Task<List<AppointmentModel>> GetInactiveAppointments(int pagenumber, int pagesize);
        public Task<List<AppointmentModel>> FilterAppointmentsByDate(DateOnly date, int pageNumber, int pageSize);
        public Task<List<AppointmentModel>> FilterAppointmentsByStatus(string status, int pageNumber, int pageSize);
        public Task CancelAppointment(int id);
        public Task<List<AppointmentModel>> GetAllAppointmentsByPatientID(int patientId, int pageNumber, int pageSize);
        public Task<List<AppointmentModel>> GetAllAppointmentsByDoctorID(int doctorId, int pageNumber, int pageSize);
    }
}
