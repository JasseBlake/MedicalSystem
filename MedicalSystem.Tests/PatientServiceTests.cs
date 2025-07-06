using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using MedicalSystem.DTOs;
using MedicalSystem.Services;
using Xunit;

namespace MedicalSystem.Tests
{
    public class PatientServiceTests : IDisposable
    {
        private readonly MedicalDbContext _context;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            var options = new DbContextOptionsBuilder<MedicalDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new MedicalDbContext(options);
            _patientService = new PatientService(_context);
        }

        [Fact]
        public async Task CreateAsync_ShouldCalculateAge_Correctly()
        {
            var patientDto = new PatientDto
            {
                FirstName = "Test",
                LastName = "Patient",
                DateOfBirth = DateTime.Now.AddYears(-25)
            };

            var result = await _patientService.CreateAsync(patientDto);

            Assert.Equal(CrudOperationResultStatus.Success, result.Status);
            Assert.Equal(25, result.Result.Age);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}