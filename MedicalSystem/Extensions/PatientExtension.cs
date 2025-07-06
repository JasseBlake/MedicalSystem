using MedicalSystem.DTOs;
using MedicalSystem.Models;

namespace MedicalSystem.Extensions
{
    public static class PatientExtension
    {
        public static PatientDto ToDto(this Patient entity)
        {
            return new PatientDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                DateOfBirth = entity.DateOfBirth
            };
        }

        public static Patient ToEntity(this PatientDto dto)
        {
            return new Patient
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth
            };
        }
    }
}