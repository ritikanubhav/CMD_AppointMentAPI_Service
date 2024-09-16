using CMD.Appointment.Data;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.OData.Query;
using CMD.Appointment.Domain.Manager;
using CMD.Appointment.Domain.Exceptions;
using CMD.Appointment.Domain.DTO;

namespace CMD.Appointment.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentManager appointmentManager;
        private readonly IMessageService messageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentController"/> class.
        /// </summary>
        /// <param name="appointmentManager">The appointment manager service.</param>
        /// <param name="messageService">The message service.</param>
        public AppointmentController(IAppointmentManager appointmentManager, IMessageService messageService)
        {
            this.appointmentManager = appointmentManager;
            this.messageService = messageService;
        }

        /// <summary>
        /// Adds a new appointment.
        /// </summary>
        /// <param name="appointment">The appointment details.</param>
        /// <returns>A <see cref="CreatedResult"/> if the appointment is created successfully; otherwise, a <see cref="BadRequestResult"/>.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAppointment(AppointmentModel appointment)
        {
            if (!ModelState.IsValid)
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

        /// <summary>
        /// Cancels an existing appointment by its ID.
        /// </summary>
        /// <param name="id">The ID of the appointment to cancel.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the cancellation operation.</returns>
        [HttpPut("Cancel/{id}")]
        [Consumes("application/json")]
        [ProducesResponseType<AppointmentModel>(StatusCodes.Status200OK)]
        [ProducesResponseType<AppointmentModel>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                await appointmentManager.CancelAppointment(id);
                return Ok(messageService.GetMessage("CompletedCancellation"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Filters appointments based on their status.
        /// </summary>
        /// <param name="status">The status to filter by.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A list of appointments matching the specified status.</returns>
        [HttpGet("FilterByStatus")]
        [ProducesResponseType<AppointmentModel>(StatusCodes.Status200OK)]
        [ProducesResponseType<AppointmentModel>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<AppointmentModel>(StatusCodes.Status404NotFound)]
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
                return BadRequest($"Invalid status value: '{status}'. Please provide a valid appointment status.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while processing your request: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing appointment.
        /// </summary>
        /// <param name="appointmentData">The updated appointment details.</param>
        /// <param name="id">The ID of the appointment to update.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
        [HttpPut]
        [Route("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentDTO appointmentData, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await appointmentManager.UpdateAppointment(appointmentData, id);
                return Ok(appointmentData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all appointments with optional pagination.
        /// </summary>
        /// <param name="pageNo">The page number for pagination.</param>
        /// <param name="pageLimit">The number of items per page.</param>
        /// <returns>A list of all appointments.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAppointments([FromQuery] int pageNo = 1, [FromQuery] int pageLimit = 20)
        {
            try
            {
                var result = await appointmentManager.GetAllAppointments(pageNo, pageLimit);
                if (result == null || result.Items.Count == 0)
                    return NotFound(messageService.GetMessage("InvalidAppointment"));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Filters appointments by date.
        /// </summary>
        /// <param name="date">The date to filter by.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A list of appointments for the specified date.</returns>
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves active appointments with optional pagination.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A list of active appointments.</returns>
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

                if (appointments == null || appointments.Count == 0)
                {
                    return NotFound("No active appointments found.");
                }

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves inactive appointments with optional pagination.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A list of inactive appointments.</returns>
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

        /// <summary>
        /// Retrieves an appointment by its ID.
        /// </summary>
        /// <param name="id">The ID of the appointment.</param>
        /// <returns>The details of the specified appointment.</returns>
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
                    return NotFound();
                }

                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all appointments for a specific patient by their ID.
        /// </summary>
        /// <param name="id">The ID of the patient.</param>
        /// <param name="pageNo">The page number for pagination.</param>
        /// <param name="pageLimit">The number of items per page.</param>
        /// <returns>A list of appointments for the specified patient.</returns>
        [HttpGet]
        [Route("patient/{id}")]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAppointmentsByPatientId(int id, [FromQuery] int pageNo = 1, [FromQuery] int pageLimit = 20)
        {
            try
            {
                var appointments = await appointmentManager.GetAllAppointmentsByPatientID(id, pageNo, pageLimit);

                if (appointments == null || appointments.Count() == 0)
                {
                    return NotFound(messageService.GetMessage("NoAppointmentsForPatientId"));
                }

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all appointments for a specific doctor by their ID.
        /// </summary>
        /// <param name="id">The ID of the doctor.</param>
        /// <param name="pageNo">The page number for pagination.</param>
        /// <param name="pageLimit">The number of items per page.</param>
        /// <returns>A list of appointments for the specified doctor.</returns>
        [HttpGet]
        [Route("doctor/{id}")]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAppointmentsByDoctorId(int id, [FromQuery] int pageNo = 1, [FromQuery] int pageLimit = 20)
        {
            try
            {
                var appointments = await appointmentManager.GetAllAppointmentsByDoctorID(id, pageNo, pageLimit);

                if (appointments == null || appointments.Count() == 0)
                {
                    return NotFound(messageService.GetMessage("NoAppointmentsForDoctorId"));
                }

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
