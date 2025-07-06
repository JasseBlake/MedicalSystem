using Microsoft.AspNetCore.Mvc;
using MedicalSystem.DTOs;
using MedicalSystem.Services;

namespace MedicalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IEnumerable<NotificationDto>> GetNotifications()
        {
            return await _notificationService.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotification(Guid id)
        {
            var notification = await _notificationService.GetByIdAsync(id);
            if (notification == null)
                return NotFound();

            return Ok(notification);
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<IEnumerable<NotificationDto>> GetNotificationsByAppointment(Guid appointmentId)
        {
            return await _notificationService.GetByAppointmentIdAsync(appointmentId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _notificationService.CreateAsync(dto);

            if (result.Status == CrudOperationResultStatus.Success)
                return Ok(result.Result);

            return BadRequest(result.ErrorMessage);
        }
    }
}