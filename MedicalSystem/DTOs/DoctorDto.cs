using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.DTOs
{
    public class DoctorDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specjalizacja jest wymagana")]
        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;


        public string FullName => $"{FirstName} {LastName}";
    }
}