using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using MedicalSystem.DTOs;
using MedicalSystem.Services;
using Xunit;

namespace MedicalSystem.Tests
{
    public class AppointmentServiceTests : IDisposable
    {
        private readonly MedicalDbContext _context;
        private readonly DoctorService _doctorService;
        private readonly PatientService _patientService;
        private readonly NotificationService _notificationService;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTests()
        {

            var options = new DbContextOptionsBuilder<MedicalDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new MedicalDbContext(options);


            _doctorService = new DoctorService(_context);
            _patientService = new PatientService(_context);
            _notificationService = new NotificationService(_context);
            _appointmentService = new AppointmentService(_context, _notificationService, _doctorService, _patientService);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnSuccess_WhenDoctorAndPatientExist()
        {

            var doctor = await _doctorService.CreateAsync(new DoctorDto
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Specialization = "Kardiolog"
            });

            var patient = await _patientService.CreateAsync(new PatientDto
            {
                FirstName = "Anna",
                LastName = "Nowak",
                DateOfBirth = new DateTime(1990, 5, 15)
            });

            var appointmentDto = new AppointmentDto
            {
                DoctorId = doctor.Result.Id,
                PatientId = patient.Result.Id,
                AppointmentDate = DateTime.Now.AddDays(7)
            };


            var result = await _appointmentService.CreateAsync(appointmentDto);


            Assert.Equal(CrudOperationResultStatus.Success, result.Status);
            Assert.NotNull(result.Result);
            Assert.Equal(doctor.Result.Id, result.Result.DoctorId);
            Assert.Equal(patient.Result.Id, result.Result.PatientId);

 
            var notifications = await _notificationService.GetAsync();
            Assert.Single(notifications); 
            Assert.Contains("Nowa wizyta", notifications.First().Title);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnFailure_WhenDoctorNotExists()
        {

            var patient = await _patientService.CreateAsync(new PatientDto
            {
                FirstName = "Anna",
                LastName = "Nowak",
                DateOfBirth = new DateTime(1990, 5, 15)
            });

            var appointmentDto = new AppointmentDto
            {
                DoctorId = Guid.NewGuid(), 
                PatientId = patient.Result.Id,
                AppointmentDate = DateTime.Now.AddDays(7)
            };

            var result = await _appointmentService.CreateAsync(appointmentDto);


            Assert.Equal(CrudOperationResultStatus.Failure, result.Status);
            Assert.Contains("Lekarz nie istnieje", result.ErrorMessage);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnFailure_WhenPatientNotExists()
        {

            var doctor = await _doctorService.CreateAsync(new DoctorDto
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Specialization = "Kardiolog"
            });

            var appointmentDto = new AppointmentDto
            {
                DoctorId = doctor.Result.Id,
                PatientId = Guid.NewGuid(), 
                AppointmentDate = DateTime.Now.AddDays(7)
            };


            var result = await _appointmentService.CreateAsync(appointmentDto);

            Assert.Equal(CrudOperationResultStatus.Failure, result.Status);
            Assert.Contains("Pacjent nie istnieje", result.ErrorMessage);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}