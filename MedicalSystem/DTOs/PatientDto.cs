using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.DTOs
{
    public class PatientDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data urodzenia jest wymagana")]
        public DateTime DateOfBirth { get; set; }


        public string FullName => $"{FirstName} {LastName}";
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
    }
}