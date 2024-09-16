using System;

namespace CMD.Appointment.Domain.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an invalid patient ID is encountered.
    /// </summary>
    public class InvalidPatientIdException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPatientIdException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidPatientIdException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPatientIdException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public InvalidPatientIdException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
