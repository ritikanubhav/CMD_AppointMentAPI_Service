using System;

namespace CMD.Appointment.Domain.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an appointment cannot be cancelled.
    /// </summary>
    public class AppointmentUnableToCancelException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentUnableToCancelException"/> class.
        /// </summary>
        public AppointmentUnableToCancelException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentUnableToCancelException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AppointmentUnableToCancelException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentUnableToCancelException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="ex">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public AppointmentUnableToCancelException(string message, Exception ex) : base(message, ex) { }
    }
}
