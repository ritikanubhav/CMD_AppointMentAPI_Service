using System;

namespace CMD.Appointment.Domain.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when pagination parameters are not valid.
    /// </summary>
    public class NotValidPaginationException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotValidPaginationException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NotValidPaginationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotValidPaginationException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public NotValidPaginationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
