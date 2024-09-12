
﻿using CMD.Appointment.Data;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.OData.Query;

namespace CMD.Appointment.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepo appointmentRepo;
        private readonly IDateValidator dateValidator;

        public AppointmentController(IAppointmentRepo appointmentRepo,IDateValidator dateValidator)
        {
            this.appointmentRepo = appointmentRepo;
            this.dateValidator= dateValidator;
        }

        // POST: api/Appointments
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]    
        [ProducesResponseType(StatusCodes.Status400BadRequest)]    
        public async Task<IActionResult> AddAppointment(AppointmentModel appointment)
        {
            //Validating properties and date
            if (!ModelState.IsValid || !dateValidator.ValidateDate(appointment.Date))
            {
                return BadRequest(ModelState);
            }
            try
            {
                await appointmentRepo.AddAppointment(appointment);
                var locationUri = $"/api/Appointments/{appointment.Id}";
                return Created(locationUri, appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT: api/Appointments/Cancel/{id}
        [HttpPut("Cancel/{id}")]
        [Consumes("appliation/json")]
        [ProducesResponseType<AppointmentModel>(StatusCodes.Status201Created)]
        [ProducesResponseType<AppointmentModel>(StatusCodes.Status200OK)]
        [ProducesResponseType<AppointmentModel>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                await appointmentRepo.CancelAppointment(id);
                return NoContent(); // Returns HTTP 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); // Handle exceptions
            }
        }

        [HttpGet("FilterByStatus")]
        [Consumes("application/json")]
        [ProducesResponseType<AppointmentModel>(StatusCodes.Status200OK)]
        [ProducesResponseType<AppointmentModel>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FilterAppointmentsByStatus(string status, int pageNumber = 1, int pageSize = 20)
        {
            var result = await appointmentRepo.FilterAppointmentsByStatus(status,pageNumber,pageSize);

            if (result == null || result.Count == 0)
                return NotFound($"No appointments found with status: {status}");

            return Ok(result);

        }

        //Put : api/appointments
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAppointment(AppointmentModel appointment)
        {
            if(!ModelState.IsValid ||!dateValidator.ValidateDate(appointment.Date))
            {
                return BadRequest(ModelState);
            }
            try
            {
                await appointmentRepo.UpdateAppointment(appointment);
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAppointments([FromQuery] int pageNo=1, [FromQuery] int pageLimit=20)
        {
            if (pageNo <= 0 || pageLimit <= 0 )
            {
                return BadRequest("Invalid pagination parameters."); // Return 400 Bad Request if pagination parameters are invalid
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 Bad Request if the model state is invalid
            }

            var appointments = await appointmentRepo.GetAllAppointments(pageNo, pageLimit);

            if (appointments == null || appointments.Count() == 0)
            {
                return NotFound("No appointments found."); // Return 404 if no appointments are found
            }

            return Ok(appointments); // Return 200 OK with the list of appointments
        }

        [HttpGet("FilterByDate")]
        public async Task<IActionResult> FilterAppointmentsByDate(DateOnly date, int pageNumber = 1, int pageSize = 20)
        {
            // Validate pagination parameters
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            // Convert DateTime to DateOnly
            var result = await appointmentRepo.FilterAppointmentsByDate(date, pageNumber, pageSize);

            if (result == null || result.Count == 0)
            {
                return NotFound("No appointments found for the specified date.");
            }

            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveAppointments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var appointments = await appointmentRepo.GetActiveAppointments(pageNumber, pageSize);

            // If no appointments are found, return 404 Not Found
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound("No inactive appointments found.");
            }

            return Ok(appointments);
        }


        [HttpGet("Inactive")]
        public async Task<IActionResult> GetInactiveAppointments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var appointments = await appointmentRepo.GetInactiveAppointments(pageNumber, pageSize);

            // If no appointments are found, return 404 Not Found
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound("No inactive appointments found.");
            }

            return Ok(appointments);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAppointmentById([FromRoute] int id)
        {
            // Validate the ID
            if (id <= 0)
            {
                return BadRequest("Invalid appointment ID. ID must be a positive integer."); // Return 400 Bad Request if the ID is invalid
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var appointment = await appointmentRepo.GetAppointmentById(id);

                if (appointment == null)
                {
                    return NotFound(); // Return 404 if not found
                }

                return Ok(appointment);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request."); // Return 500 Internal Server Error if an exception occurs
            }
        }
    }
}
