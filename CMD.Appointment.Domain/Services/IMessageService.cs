using System;

namespace CMD.Appointment.Domain.Services
{
    /// <summary>
    /// Defines a contract for a service that provides messages based on a given key.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Retrieves a message corresponding to the specified key.
        /// </summary>
        /// <param name="key">The key associated with the desired message.</param>
        /// <returns>The message associated with the specified key.</returns>
        string GetMessage(string key);
    }
}
