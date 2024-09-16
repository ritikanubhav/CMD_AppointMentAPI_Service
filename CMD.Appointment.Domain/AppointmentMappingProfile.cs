using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CMD.Appointment.Domain.DTO;
using CMD.Appointment.Domain.Entities;

namespace CMD.Appointment.Domain
{
    /// <summary>
    /// AutoMapper profile for configuring mappings between appointment-related objects.
    /// </summary>
    public class AppointmentMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentMappingProfile"/> class.
        /// Configures the mapping rules for appointment-related objects.
        /// </summary>
        public AppointmentMappingProfile()
        {
            // Map from AppointmentModel to AppointmentModel (self-mapping)
            CreateMap<AppointmentModel, AppointmentModel>();

            // Map from UpdateAppointmentDTO to AppointmentModel
            CreateMap<UpdateAppointmentDTO, AppointmentModel>();
        }
    }
}
