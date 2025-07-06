using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using MedicalSystem.DTOs;
using MedicalSystem.Services;
using Xunit;

namespace MedicalSystem.Tests
{
    public class DoctorServiceTests : IDisposable
    {
        private readonly MedicalDbContext _context;
        private readonly DoctorService _doctorService;

        public DoctorServiceTests()
        {
            var options = new DbContextOptionsBuilder<MedicalDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new MedicalDbContext(options);
            _doctorService = new DoctorService(_context);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnSuccess_WhenValidDoctor()
        {
            var doctorDto = new DoctorDto
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Specialization = "Kardiolog"
            };

            var result = await _doctorService.CreateAsync(doctorDto);

            Assert.Equal(CrudOperationResultStatus.Success, result.Status);
            Assert.NotNull(result.Result);
            Assert.Equal("Jan", result.Result.FirstName);
            Assert.Equal("Kardiolog", result.Result.Specialization);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDoctor_WhenExists()
        {
            var doctorDto = new DoctorDto
            {
                FirstName = "Anna",
                LastName = "Nowak",
                Specialization = "Dermatolog"
            };

            var created = await _doctorService.CreateAsync(doctorDto);

            var result = await _doctorService.GetByIdAsync(created.Result.Id);

            Assert.NotNull(result);
            Assert.Equal("Anna", result.FirstName);
            Assert.Equal("Dermatolog", result.Specialization);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var nonExistentId = Guid.NewGuid();

            var result = await _doctorService.GetByIdAsync(nonExistentId);

            Assert.Null(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}