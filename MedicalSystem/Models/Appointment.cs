using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSystem.Models
{
    [Table("Appointments")]
    public class Appointment : BaseEntity
    {
        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public Guid PatientId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }


        public virtual Doctor Doctor { get; set; } = null!;
        public virtual Patient Patient { get; set; } = null!;
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}