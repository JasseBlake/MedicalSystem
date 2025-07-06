using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using MedicalSystem.DTOs;
using MedicalSystem.Extensions;
using MedicalSystem.Models;

namespace MedicalSystem.Services
{
    public class DoctorService
    {
        private readonly MedicalDbContext _context;

        public DoctorService(MedicalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoctorDto>> GetAsync()
        {
            var doctors = await _context.Doctors
                .AsNoTracking()
                .Select(e => e.ToDto())
                .ToListAsync();
            return doctors;
        }

        public async Task<DoctorDto?> GetByIdAsync(Guid id)
        {
            var doctor = await _context.Doctors
                .AsNoTracking()
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            return doctor?.ToDto();
        }

        public async Task<CrudOperationResult<DoctorDto>> CreateAsync(DoctorDto dto)
        {
            try
            {
                var entity = dto.ToEntity();
                entity.Id = Guid.NewGuid();

                _context.Doctors.Add(entity);
                await _context.SaveChangesAsync();

                var newDto = await GetByIdAsync(entity.Id);

                return new CrudOperationResult<DoctorDto>
                {
                    Result = newDto,
                    Status = CrudOperationResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new CrudOperationResult<DoctorDto>
                {
                    Status = CrudOperationResultStatus.Failure,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<CrudOperationResult<DoctorDto>> UpdateAsync(Guid id, DoctorDto dto)
        {
            try
            {
                var existingDoctor = await _context.Doctors
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (existingDoctor == null)
                {
                    return new CrudOperationResult<DoctorDto>
                    {
                        Status = CrudOperationResultStatus.RecordNotFound
                    };
                }

                existingDoctor.FirstName = dto.FirstName;
                existingDoctor.LastName = dto.LastName;
                existingDoctor.Specialization = dto.Specialization;

                await _context.SaveChangesAsync();

                var updatedDto = await GetByIdAsync(id);
                return new CrudOperationResult<DoctorDto>
                {
                    Result = updatedDto,
                    Status = CrudOperationResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new CrudOperationResult<DoctorDto>
                {
                    Status = CrudOperationResultStatus.Failure,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<CrudOperationResult<DoctorDto>> DeleteAsync(Guid id)
        {
            try
            {
                var doctor = await _context.Doctors
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (doctor == null)
                {
                    return new CrudOperationResult<DoctorDto>
                    {
                        Status = CrudOperationResultStatus.RecordNotFound
                    };
                }

                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();

                return new CrudOperationResult<DoctorDto>
                {
                    Status = CrudOperationResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new CrudOperationResult<DoctorDto>
                {
                    Status = CrudOperationResultStatus.Failure,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}