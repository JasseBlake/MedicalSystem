using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using MedicalSystem.DTOs;
using MedicalSystem.Extensions;
using MedicalSystem.Models;

namespace MedicalSystem.Services
{
    public class NotificationService
    {
        private readonly MedicalDbContext _context;

        public NotificationService(MedicalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NotificationDto>> GetAsync()
        {
            var notifications = await _context.Notifications
                .Include(n => n.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Include(n => n.Appointment)
                    .ThenInclude(a => a.Patient)
                .AsNoTracking()
                .OrderByDescending(n => n.Id)
                .Select(e => e.ToDto())
                .ToListAsync();
            return notifications;
        }

        public async Task<NotificationDto?> GetByIdAsync(Guid id)
        {
            var notification = await _context.Notifications
                .Include(n => n.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Include(n => n.Appointment)
                    .ThenInclude(a => a.Patient)
                .AsNoTracking()
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            return notification?.ToDto();
        }

        public async Task<CrudOperationResult<NotificationDto>> CreateAsync(NotificationDto dto)
        {
            try
            {
                var entity = dto.ToEntity();
                entity.Id = Guid.NewGuid();

                _context.Notifications.Add(entity);
                await _context.SaveChangesAsync();

                var newDto = await GetByIdAsync(entity.Id);

                return new CrudOperationResult<NotificationDto>
                {
                    Result = newDto,
                    Status = CrudOperationResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new CrudOperationResult<NotificationDto>
                {
                    Status = CrudOperationResultStatus.Failure,
                    ErrorMessage = ex.Message
                };
            }
        }


        public async Task CreateAppointmentNotificationAsync(Guid appointmentId, string title, string message)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                AppointmentId = appointmentId,
                Title = title,
                Message = message
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationDto>> GetByAppointmentIdAsync(Guid appointmentId)
        {
            var notifications = await _context.Notifications
                .Include(n => n.Appointment)
                    .ThenInclude(a => a.Doctor)
                .Include(n => n.Appointment)
                    .ThenInclude(a => a.Patient)
                .Where(n => n.AppointmentId == appointmentId)
                .AsNoTracking()
                .Select(e => e.ToDto())
                .ToListAsync();
            return notifications;
        }
    }
}