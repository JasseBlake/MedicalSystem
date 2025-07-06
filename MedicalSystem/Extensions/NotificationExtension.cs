using MedicalSystem.DTOs;
using MedicalSystem.Models;

namespace MedicalSystem.Extensions
{
    public static class NotificationExtension
    {
        public static NotificationDto ToDto(this Notification entity)
        {
            return new NotificationDto
            {
                Id = entity.Id,
                AppointmentId = entity.AppointmentId,
                Title = entity.Title,
                Message = entity.Message,
                DoctorName = entity.Appointment?.Doctor != null ?
                    $"{entity.Appointment.Doctor.FirstName} {entity.Appointment.Doctor.LastName}" : null,
                PatientName = entity.Appointment?.Patient != null ?
                    $"{entity.Appointment.Patient.FirstName} {entity.Appointment.Patient.LastName}" : null,
                AppointmentDate = entity.Appointment?.AppointmentDate
            };
        }

        public static Notification ToEntity(this NotificationDto dto)
        {
            return new Notification
            {
                Id = dto.Id,
                AppointmentId = dto.AppointmentId,
                Title = dto.Title,
                Message = dto.Message
            };
        }
    }
}