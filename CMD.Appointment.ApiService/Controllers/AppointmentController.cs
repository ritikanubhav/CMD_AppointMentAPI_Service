using CMD.Appointment.Data;
using CMD.Appointment.Domain.Entities;
using CMD.Appointment.Domain.IRepositories;
using CMD.Appointment.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using CMD.Appointment.Domain.Manager;
using CMD.Appointment.Domain.Exceptions;
using CMD.Appointment.Domain.DTO;
using NLog;

namespace CMD.Appointment.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IAppointmentManager appointmentManager;
        private readonly IMessageService messageService;

        public AppointmentController(IAppointmentManager appointmentManager, IMessageService messageService)
        {
            this.appointmentManager = appointmentManager;
            this.messageService = messageService;
        }

        // POST: api/Appointments
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
                _logger.Error(ex, "Error occurred while adding an appointment.");
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Appointments/Cancel/{id}
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
                _logger.Warn(ex, $"Appointment with ID {id} not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while cancelling appointment with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

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
                {
                    return NotFound($"No appointments found with status: {status}");
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, $"Invalid status value: '{status}'.");
                return BadRequest($"Invalid status value: '{status}'. Please provide a valid appointment status.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while filtering appointments by status.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
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
                _logger.Error(ex, $"Error occurred while updating appointment with ID {id}.");
                return BadRequest(ex.Message);
            }
        }

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
                {
                    return NotFound(messageService.GetMessage("InvalidAppointment"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while retrieving all appointments.");
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
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while filtering appointments by date {date}.");
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
                if (appointments == null || appointments.Count == 0)
                {
                    return NotFound("No active appointments found.");
                }
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while retrieving active appointments.");
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
                if (appointments == null || appointments.Count == 0)
                {
                    return NotFound(messageService.GetMessage("NoInActiveAppointments"));
                }
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while retrieving inactive appointments.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
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
                _logger.Error(ex, $"Error occurred while retrieving appointment with ID {id}.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("patient/{id}")]
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
                _logger.Error(ex, $"Error occurred while retrieving appointments for patient with ID {id}.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("doctor/{id}")]
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
                _logger.Error(ex, $"Error occurred while retrieving appointments for doctor with ID {id}.");
                return BadRequest(ex.Message);
            }
        }
    }
}
