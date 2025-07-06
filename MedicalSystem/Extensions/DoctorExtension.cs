using MedicalSystem.DTOs;
using MedicalSystem.Models;

namespace MedicalSystem.Extensions
{
    public static class DoctorExtension
    {
        public static DoctorDto ToDto(this Doctor entity)
        {
            return new DoctorDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Specialization = entity.Specialization
            };
        }

        public static Doctor ToEntity(this DoctorDto dto)
        {
            return new Doctor
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Specialization = dto.Specialization
            };
        }
    }
}