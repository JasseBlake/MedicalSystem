using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.DTOs
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }

        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public Guid PatientId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }


        public string? DoctorName { get; set; }
        public string? PatientName { get; set; }
    }
}