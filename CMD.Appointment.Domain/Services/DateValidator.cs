using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Appointment.Domain.Services
{
    /// <summary>
    /// Provides methods to validate dates for appointment-related operations.
    /// </summary>
    public class DateValidator
    {
        /// <summary>
        /// Validates if the given date is within the acceptable range (today to 30 days in the future).
        /// </summary>
        /// <param name="date">The date to validate.</param>
        /// <returns><c>true</c> if the date is valid; otherwise, <c>false</c>.</returns>
        public static bool ValidateDate(DateOnly date)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            DateOnly maxDate = today.AddDays(30);

            if (date < today)
            {
                // Date is in the past
                return false;
            }
            else if (date > maxDate)
            {
                // Date is more than 30 days from now
                return false;
            }
            // Date is valid
            return true;
        }
    }
}
