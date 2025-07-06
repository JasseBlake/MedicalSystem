using MedicalSystem.DTOs;
using MedicalSystem.Models;

namespace MedicalSystem.Extensions
{
    public static class AppointmentExtension
    {
        public static AppointmentDto ToDto(this Appointment entity)
        {
            return new AppointmentDto
            {
                Id = entity.Id,
                DoctorId = entity.DoctorId,
                PatientId = entity.PatientId,
                AppointmentDate = entity.AppointmentDate,
                DoctorName = entity.Doctor != null ? $"{entity.Doctor.FirstName} {entity.Doctor.LastName}" : null,
                PatientName = entity.Patient != null ? $"{entity.Patient.FirstName} {entity.Patient.LastName}" : null
            };
        }

        public static Appointment ToEntity(this AppointmentDto dto)
        {
            return new Appointment
            {
                Id = dto.Id,
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                AppointmentDate = dto.AppointmentDate
            };
        }
    }
}