using System;

namespace CMD.Appointment.Domain.Services
{
    /// <summary>
    /// Provides functionality to validate pagination parameters.
    /// </summary>
    public static class PaginationValidator
    {
        /// <summary>
        /// Validates pagination parameters to ensure they are within acceptable ranges.
        /// </summary>
        /// <param name="pageNo">The page number to validate.</param>
        /// <param name="pageSize">The page size to validate.</param>
        /// <returns>Returns <c>true</c> if both the page number and page size are valid; otherwise, returns <c>false</c>.</returns>
        public static bool ValidatePagination(int pageNo, int pageSize)
        {
            if (pageNo > 0 && (pageSize > 0 && pageSize <= 100))
            {
                return true;
            }
            return false;
        }
    }
}
