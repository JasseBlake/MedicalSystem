using Microsoft.EntityFrameworkCore;
using MedicalSystem.Models;

namespace MedicalSystem.Data
{
    public class MedicalDbContext : DbContext
    {
        public MedicalDbContext(DbContextOptions<MedicalDbContext> options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasMany(d => d.Appointments)
                    .WithOne(a => a.Doctor)
                    .HasForeignKey(a => a.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasMany(p => p.Appointments)
                    .WithOne(a => a.Patient)
                    .HasForeignKey(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasMany(a => a.Notifications)
                    .WithOne(n => n.Appointment)
                    .HasForeignKey(n => n.AppointmentId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
            });


            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {

            var doctor1Id = Guid.NewGuid();
            var doctor2Id = Guid.NewGuid();

            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = doctor1Id, FirstName = "Jan", LastName = "Kowalski", Specialization = "Kardiolog" },
                new Doctor { Id = doctor2Id, FirstName = "Anna", LastName = "Nowak", Specialization = "Dermatolog" }
            );


            var patient1Id = Guid.NewGuid();
            var patient2Id = Guid.NewGuid();

            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = patient1Id, FirstName = "Piotr", LastName = "Zieliński", DateOfBirth = new DateTime(1985, 5, 15) },
                new Patient { Id = patient2Id, FirstName = "Maria", LastName = "Wójcik", DateOfBirth = new DateTime(1990, 8, 22) }
            );
        }
    }
}