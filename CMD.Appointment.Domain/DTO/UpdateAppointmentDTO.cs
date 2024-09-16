using System;
using System.ComponentModel.DataAnnotations;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.Services;

namespace CMD.Appointment.Domain.DTO
{
    /// <summary>
    /// Represents the data transfer object used to update appointment details.
    /// </summary>
    public class UpdateAppointmentDTO
    {
        /// <summary>
        /// Gets or sets the purpose of the visit.
        /// </summary>
        /// <remarks>
        /// Must be a string, with a minimum length of 1 character and a maximum length of 255 characters.
        /// This field is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "Purpose of visit is mandatory.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Purpose of visit must be between 1 and 255 characters.")]
        public string PurposeOfVisit { get; set; }

        /// <summary>
        /// Gets or sets the date of the appointment.
        /// </summary>
        /// <remarks>
        /// Must be of type DateOnly and is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "Date is mandatory.")]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Gets or sets the time of the appointment.
        /// </summary>
        /// <remarks>
        /// Must be of type TimeOnly and is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "Time is mandatory.")]
        public TimeOnly Time { get; set; }

        /// <summary>
        /// Gets or sets the email address of the person associated with the appointment.
        /// </summary>
        /// <remarks>
        /// Must be a valid email address format and is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "Email is mandatory.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the person associated with the appointment.
        /// </summary>
        /// <remarks>
        /// Must be a valid phone number format. For India, it should include the country code "91".
        /// This field is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "Phone is mandatory.")]
        [PhoneNumberValidator("91")]  // Validates phone number for India ("91")
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets a message related to the appointment.
        /// </summary>
        /// <remarks>
        /// Must be a string, with a minimum length of 1 character and a maximum length of 255 characters.
        /// This field is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "Message is mandatory.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Message must be between 1 and 255 characters.")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the user who last modified the appointment.
        /// </summary>
        /// <remarks>
        /// Must be a string and is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "LastModifiedBy is mandatory.")]
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the status of the appointment.
        /// </summary>
        /// <remarks>
        /// Must be an enum of type AppointmentStatus. The status can be one of the predefined values (e.g., Scheduled, Cancelled, Closed).
        /// </remarks>
        [EnumDataType(typeof(AppointmentStatus))]
        public AppointmentStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the patient associated with the appointment.
        /// </summary>
        /// <remarks>
        /// This field is optional.
        /// </remarks>
        public int? PatientId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the doctor associated with the appointment.
        /// </summary>
        /// <remarks>
        /// This field is optional.
        /// </remarks>
        public int? DoctorId { get; set; }
    }
}
