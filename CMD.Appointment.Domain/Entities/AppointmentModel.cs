using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.Services;

namespace CMD.Appointment.Domain.Entities
{
    public class AppointmentModel
    {
        // ID: Must be system generated and auto-incremented (handled at the database level)
        [Key] // Specifies that this property is the primary key
        [Required] // Ensures that the ID is always provided
        public int Id { get; set; }

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

        // Status: Enum type with predefined values
        [Required(ErrorMessage = "Status is mandatory.")]
        [EnumDataType(typeof(AppointmentStatus))]
        public AppointmentStatus Status { get; set; }

        // Message: Must be a string, minimum 1 character, maximum 255 characters, and mandatory
        [Required(ErrorMessage = "Message is mandatory.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Message must be between 1 and 255 characters.")]
        public string Message { get; set; }

        public string CreatedBy { get; set; }

        // CreatedDate: Must be a datetime and mandatory
        [Required(ErrorMessage = "CreatedDate is mandatory.")]
        public DateTime? CreatedDate { get; set; }= DateTime.Now;

        // LastModifiedBy: Must be a string and mandatory
        [Required(ErrorMessage = "LastModifiedBy is mandatory.")]
        public string LastModifiedBy { get; set; }

        // LastModifiedDate: Must be a datetime and mandatory
        [Required(ErrorMessage = "LastModifiedDate is mandatory.")]
        public DateTime? LastModifiedDate { get; set; }=DateTime.Now;

        // PatientId: 
        [Required(ErrorMessage = "PatientId is mandatory.")]
        public int PatientId { get; set; }

        // DoctorId:
        [Required(ErrorMessage = "DoctorId is mandatory.")]
        public int DoctorId { get; set; }
    }
}
