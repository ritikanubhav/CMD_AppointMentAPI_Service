using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Appointment.Domain.Services
{
    public class DoctorAvailabiltyValidator
    {
        private static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://cmd-doctor-api.azurewebsites.net")
        };

        /// <summary>
        /// Validates if the given doctor is available for date and time of appointment
        /// </summary>
        /// <param name="DoctorId">The doctor ID to validate.</param>
        /// <param name="DoctorId">The doctor ID to validate.</param>
        /// <param name="DoctorId">The doctor ID to validate.</param>
        /// <returns>True if the doctor ID exists, otherwise false.</returns>
        public static async Task<bool> ValidateDoctorAvailability(int doctorId,DateOnly date,TimeOnly time)
        {

            try
            {
                // Make the API call to check if the patient ID exists
                var response = await HttpClient.GetAsync($"/api/Doctor/{doctorId}");

                // Return true if the status code is success (e.g., 200 OK)
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine($"An error occurred while validating the  ID: {ex.Message}");
                return false;
            }
        }
    }
}
