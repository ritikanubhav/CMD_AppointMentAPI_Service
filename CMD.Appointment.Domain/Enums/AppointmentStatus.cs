using System;

namespace CMD.Appointment.Domain.Enums
{
    /// <summary>
    /// Defines the possible statuses for an appointment.
    /// </summary>
    public enum AppointmentStatus
    {
        /// <summary>
        /// Indicates that the appointment is scheduled.
        /// </summary>
        SCHEDULED = 0,

        /// <summary>
        /// Indicates that the appointment has been cancelled.
        /// </summary>
        CANCELLED = 1,

        /// <summary>
        /// Indicates that the appointment is completed and closed.
        /// </summary>
        CLOSED = 2
    }
}
