using System.ComponentModel.DataAnnotations;

namespace MedicalSystem.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}