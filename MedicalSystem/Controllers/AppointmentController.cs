using Microsoft.AspNetCore.Mvc;
using MedicalSystem.DTOs;
using MedicalSystem.Services;

namespace MedicalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IEnumerable<AppointmentDto>> GetAppointments()
        {
            return await _appointmentService.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(Guid id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
                return NotFound();

            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _appointmentService.CreateAsync(dto);

            if (result.Status == CrudOperationResultStatus.Success)
                return Ok(result.Result);

            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var result = await _appointmentService.DeleteAsync(id);

            return result.Status switch
            {
                CrudOperationResultStatus.Success => NoContent(),
                CrudOperationResultStatus.RecordNotFound => NotFound(),
                _ => BadRequest(result.ErrorMessage)
            };
        }
    }
}