using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CMD.Appointment.Domain.Services
{
    public static class PatientValidator
    {
        private static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://cmdpatientmodulewebapp-d8b6hvgxesa8fmda.southeastasia-01.azurewebsites.net") // Replace with your API base URL
        };

        /// <summary>
        /// Validates if the given patient ID exists in the external database.
        /// </summary>
        /// <param name="patientId">The patient ID to validate.</param>
        /// <returns>True if the patient ID exists, otherwise false.</returns>
        public static async Task<bool> ValidatePatientIdAsync(int patientId)
        {

            try
            {
                // Make the API call to check if the patient ID exists
                var response = await HttpClient.GetAsync($"/api/Patients/{patientId}");

                // Return true if the status code is success (e.g., 200 OK)
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine($"An error occurred while validating the patient ID: {ex.Message}");
                return false;
            }
        }
    }
}
