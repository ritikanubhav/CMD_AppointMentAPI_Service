
﻿using CMD.Appointment.Data;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.OData.Query;
using CMD.Appointment.Domain.Manager;

namespace CMD.Appointment.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentManager appointmentManager;
        private readonly IMessageService messageService;

        public AppointmentController(IAppointmentManager appointmentManager,IMessageService messageService)
        {
            this.appointmentManager = appointmentManager;
            this.messageService = messageService;
        }

        // POST: api/Appointments
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]    
        [ProducesResponseType(StatusCodes.Status400BadRequest)]    
        public async Task<IActionResult> AddAppointment(AppointmentModel appointment)
        {
            //Validating properties 
            if (!ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }
            try
            {
                await appointmentManager.CreateAppointment(appointment);
                var locationUri = $"/api/Appointment/{appointment.Id}";
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
                await appointmentManager.CancelAppointment(id);
                return NoContent(); // Returns HTTP 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); // Handle exceptions
            }
        }

        [HttpGet("FilterAppointmentsByStatus")]
        [Consumes("application/json")]
        [ProducesResponseType<AppointmentModel>(StatusCodes.Status200OK)]
        [ProducesResponseType<AppointmentModel>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FilterAppointmentsByStatus(string status, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var result = await appointmentManager.FilterAppointmentsByStatus(status, pageNumber, pageSize);
                if (result == null || result.Count == 0)
                    return NotFound($"No appointments found with status: {status}");
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                // Handle invalid enum parsing
                return BadRequest($"Invalid status value: '{status}'. Please provide a valid appointment status.");
            }
            catch (Exception ex)
            {
                // General exception handling
                return BadRequest($"An error occurred while processing your request: {ex.Message}");
            }
        }

        //Put : api/appointments/{id}
        [HttpPut]
        [Route("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAppointment(AppointmentModel appointmentData,int id)
        {
            if(appointmentData==null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await appointmentManager.UpdateAppointment(appointmentData,id);
                return Ok(appointmentData);
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
            try
            {
                var result = await appointmentManager.GetAllAppointments(pageNo, pageLimit);
                if (result == null || result.Count == 0)
                    return NotFound(messageService.GetMessage("InvalidAppointment"));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("FilterByDate")]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FilterAppointmentsByDate(DateOnly date, int pageNumber = 1, int pageSize = 20)
        {
            try
            {
                var result = await appointmentManager.FilterAppointmentsByDate(date, pageNumber, pageSize);

                if (result == null || result.Count == 0)
                {
                    return NotFound(messageService.GetMessage("NoAppointmentsForDate"));
                }

                return Ok(result);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("active")]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetActiveAppointments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var appointments = await appointmentManager.GetActiveAppointments(pageNumber, pageSize);

                // If no appointments are found, return 404 Not Found
                if (appointments == null || appointments.Count == 0)
                {
                    return NotFound(messageService.GetMessage("NoActiveAppointments"));
                }

                return Ok(appointments);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("Inactive")]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInactiveAppointments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var appointments = await appointmentManager.GetInactiveAppointments(pageNumber, pageSize);

                // If no appointments are found, return 404 Not Found
                if (appointments == null || appointments.Count == 0)
                {
                    return NotFound(messageService.GetMessage("NoInActiveAppointments"));
                }

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            try
            {
                var appointment = await appointmentManager.GetAppointmentById(id);

                if (appointment == null)
                {
                    return NotFound(); // Return 404 if not found
                }

                return Ok(appointment);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet]
        [Route("patient/{id}")]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAppointmentsByPatientId(int patientId,[FromQuery] int pageNo = 1, [FromQuery] int pageLimit = 20)
        {
            try
            {
                var appointments = await appointmentManager.GetAllAppointmentsByPatientID(patientId,pageNo, pageLimit);

                if (appointments == null || appointments.Count() == 0)
                {
                    return NotFound(messageService.GetMessage("NoAppointmentsForPatientId")); // Return 404 if no appointments are found
                }

                return Ok(appointments); // Return 200 OK with the list of appointments

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("doctor/{id}")]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAppointmentsByDoctorId(int doctorId, [FromQuery] int pageNo = 1, [FromQuery] int pageLimit = 20)
        {
            try
            {
                var appointments = await appointmentManager.GetAllAppointmentsByDoctorID(doctorId, pageNo, pageLimit);

                if (appointments == null || appointments.Count() == 0)
                {
                    return NotFound(messageService.GetMessage("NoAppointmentsForDoctorId")); // Return 404 if no appointments are found
                }

                return Ok(appointments); // Return 200 OK with the list of appointments

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
