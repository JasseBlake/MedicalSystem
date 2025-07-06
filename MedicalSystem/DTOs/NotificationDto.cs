using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.DTOs
{
    public class NotificationDto
    {
        public Guid Id { get; set; }

        [Required]
        public Guid AppointmentId { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wiadomość jest wymagana")]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;


        public string? DoctorName { get; set; }
        public string? PatientName { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }
}