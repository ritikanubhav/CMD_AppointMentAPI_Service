using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.Services;

namespace CMD.Appointment.Domain.DTO
{
    public class UpdateAppointmentDTO
    {
            // PurposeOfVisit: Must be a string, minimum 1 character, maximum 255 characters, and mandatory
            [Required(ErrorMessage = "Purpose of visit is mandatory.")]
            [StringLength(255, MinimumLength = 1, ErrorMessage = "Purpose of visit must be between 1 and 255 characters.")]
            public string PurposeOfVisit { get; set; }

            // Date: Must be a string and mandatory
            [Required(ErrorMessage = "Date is mandatory.")]
            public DateOnly Date { get; set; }

            // Time: Must be a TIME type and mandatory
            [Required(ErrorMessage = "Time is mandatory.")]
            public TimeOnly Time { get; set; }

            // Email: Must be a valid email address and mandatory
            [Required(ErrorMessage = "Email is mandatory.")]
            [EmailAddress(ErrorMessage = "Invalid email address format.")]
            public string Email { get; set; }

            // Phone: Must be a valid phone number format
            [Required(ErrorMessage = "Phone is mandatory.")]
            [PhoneNumberValidator("91")]  // Validates phone number for India ("91")
            public string Phone { get; set; }

            // Message: Must be a string, minimum 1 character, maximum 255 characters, and mandatory
            [Required(ErrorMessage = "Message is mandatory.")]
            [StringLength(255, MinimumLength = 1, ErrorMessage = "Message must be between 1 and 255 characters.")]
            public string Message { get; set; }


            // LastModifiedBy: Must be a string and mandatory
            [Required(ErrorMessage = "LastModifiedBy is mandatory.")]
            public string LastModifiedBy { get; set; }

    }
}
