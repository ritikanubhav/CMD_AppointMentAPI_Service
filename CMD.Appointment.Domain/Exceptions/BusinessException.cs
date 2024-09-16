using System;

namespace CMD.Appointment.Domain.Exceptions
{
    /// <summary>
    /// Represents a general business exception that is thrown for errors related to business logic.
    /// </summary>
    public class BusinessException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        public BusinessException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BusinessException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="ex">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public BusinessException(string message, Exception ex) : base(message, ex) { }
    }
}
