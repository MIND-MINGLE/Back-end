using Application.Interface;
using Application.Request.Appointment;
using Application.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentRequest request)
        {
            var response = await _appointmentService.CreateAppointmentAsync(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> GetAppointmentById(string appointmentId)
        {
            var response = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAppointment()
        {
            var response = await _appointmentService.GetAllAppointment(); ;
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatientId(string patientId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _appointmentService.GetAppointmentsByPatientIdAsync(patientId, pageIndex, pageSize);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("therapist/{therapistId}")]
        public async Task<IActionResult> GetAppointmentsByTherapistId(string therapistId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _appointmentService.GetAppointmentsByTherapistIdAsync(therapistId, pageIndex, pageSize);
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpGet("{therapistId}/{patientId}")]
        public async Task<IActionResult> GetCurrentAppointments(string therapistId,string patientId)
        {
            var response = await _appointmentService.GetCurrentAppointments(therapistId, patientId);
            return StatusCode((int)response.StatusCode, response);
        }
        

        [HttpPut("{appointmentId}")]
        public async Task<IActionResult> UpdateAppointment(string appointmentId, [FromBody] AppointmentUpdateRequest request)
        {
            var response = await _appointmentService.UpdateAppointmentAsync(appointmentId, request);
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpPut("status/{appointmentId}")]
        public async Task<IActionResult> UpdateAppointmentStatus(string appointmentId, [FromBody] AppointmentUpdateStatus request)
        {
            var response = await _appointmentService.UpdateAppointmentStatusAsync(appointmentId, request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{appointmentId}")]
        public async Task<IActionResult> DeleteAppointment(string appointmentId)
        {
            var response = await _appointmentService.DeleteAppointmentAsync(appointmentId);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}