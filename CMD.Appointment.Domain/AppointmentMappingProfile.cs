using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMD.Appointment.Domain.Entities;

namespace CMD.Appointment.Domain
{
    public class AppointmentMappingProfile : Profile
    {
        public AppointmentMappingProfile()
        {
            // Map from Appointment to Appointment (self-mapping)
            CreateMap<AppointmentModel, AppointmentModel>();
        }
    }
}
