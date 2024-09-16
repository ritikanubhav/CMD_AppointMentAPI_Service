using System;
using System.ComponentModel.DataAnnotations;
using CMD.Appointment.Domain.Enums;
using CMD.Appointment.Domain.Services;

namespace CMD.Appointment.Domain.Entities
{
    /// <summary>
    /// Represents an appointment entity with details including identifiers, dates, and other relevant information.
    /// </summary>
    public class AppointmentModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the appointment.
        /// </summary>
        /// <remarks>
        /// This property is auto-generated and auto-incremented by the database.
        /// </remarks>
        [Key] // Specifies that this property is the primary key
        [Required] // Ensures that the ID is always provided
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the purpose of the visit for the appointment.
        /// </summary>
        /// <remarks>
        /// Must be a string with a minimum length of 1 character and a maximum length of 255 characters.
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
        /// Gets or sets the email address associated with the appointment.
        /// </summary>
        /// <remarks>
        /// Must be a valid email address format and is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "Email is mandatory.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number associated with the appointment.
        /// </summary>
        /// <remarks>
        /// Must be a valid phone number format. For India, it should include the country code "91".
        /// This field is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "Phone is mandatory.")]
        [PhoneNumberValidator("91")] // Validates phone number for India ("91")
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the status of the appointment.
        /// </summary>
        /// <remarks>
        /// Must be an enum of type AppointmentStatus. The status can be one of the predefined values (e.g., Scheduled, Cancelled, Closed).
        /// </remarks>
        [Required(ErrorMessage = "Status is mandatory.")]
        [EnumDataType(typeof(AppointmentStatus))]
        public AppointmentStatus Status { get; set; }

        /// <summary>
        /// Gets or sets a message related to the appointment.
        /// </summary>
        /// <remarks>
        /// Must be a string with a minimum length of 1 character and a maximum length of 255 characters.
        /// This field is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "Message is mandatory.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Message must be between 1 and 255 characters.")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the user who created the appointment.
        /// </summary>
        /// <remarks>
        /// This field is optional.
        /// </remarks>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the appointment was created.
        /// </summary>
        /// <remarks>
        /// Must be of type DateTime and is mandatory. Defaults to the current date and time.
        /// </remarks>
        [Required(ErrorMessage = "CreatedDate is mandatory.")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the user who last modified the appointment.
        /// </summary>
        /// <remarks>
        /// This field is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "LastModifiedBy is mandatory.")]
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the appointment was last modified.
        /// </summary>
        /// <remarks>
        /// Must be of type DateTime and is mandatory. Defaults to the current date and time.
        /// </remarks>
        [Required(ErrorMessage = "LastModifiedDate is mandatory.")]
        public DateTime? LastModifiedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the unique identifier of the patient associated with the appointment.
        /// </summary>
        /// <remarks>
        /// This field is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "PatientId is mandatory.")]
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the doctor associated with the appointment.
        /// </summary>
        /// <remarks>
        /// This field is mandatory.
        /// </remarks>
        [Required(ErrorMessage = "DoctorId is mandatory.")]
        public int DoctorId { get; set; }
    }
}
