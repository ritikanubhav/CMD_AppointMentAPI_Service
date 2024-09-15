using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Appointment.Domain.Entities
{
    public class AppointmentResponse
    {
        [Required]
        public List<AppointmentModel> Items { get; set; }
        [Required]
        public int TotalAppointments {  get; set; }
        [Required]
        public int PageNumber {  get; set; }
        [Required]
        public int PageLimit { get; set; }
    }
}
