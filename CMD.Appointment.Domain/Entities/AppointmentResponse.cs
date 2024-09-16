using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMD.Appointment.Domain.Entities
{
    /// <summary>
    /// Represents the response for an appointment request, containing a list of appointments and pagination information.
    /// </summary>
    public class AppointmentResponse
    {
        /// <summary>
        /// Gets or sets the list of appointment models included in the response.
        /// </summary>
        /// <remarks>
        /// This property is mandatory and must be provided.
        /// </remarks>
        [Required]
        public List<AppointmentModel> Items { get; set; }

        /// <summary>
        /// Gets or sets the total number of appointments available.
        /// </summary>
        /// <remarks>
        /// This property is mandatory and must be provided.
        /// </remarks>
        [Required]
        public int TotalAppointments { get; set; }

        /// <summary>
        /// Gets or sets the current page number of the paginated results.
        /// </summary>
        /// <remarks>
        /// This property is mandatory and must be provided.
        /// </remarks>
        [Required]
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page in the paginated results.
        /// </summary>
        /// <remarks>
        /// This property is mandatory and must be provided.
        /// </remarks>
        [Required]
        public int PageLimit { get; set; }
    }
}
