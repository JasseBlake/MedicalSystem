using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSystem.Models
{
    [Table("Notifications")]
    public class Notification : BaseEntity
    {
        [Required]
        public Guid AppointmentId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;


        public virtual Appointment Appointment { get; set; } = null!;
    }
}