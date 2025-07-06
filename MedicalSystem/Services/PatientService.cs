using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using MedicalSystem.DTOs;
using MedicalSystem.Extensions;
using MedicalSystem.Models;

namespace MedicalSystem.Services
{
    public class PatientService
    {
        private readonly MedicalDbContext _context;

        public PatientService(MedicalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PatientDto>> GetAsync()
        {
            var patients = await _context.Patients
                .AsNoTracking()
                .Select(e => e.ToDto())
                .ToListAsync();
            return patients;
        }

        public async Task<PatientDto?> GetByIdAsync(Guid id)
        {
            var patient = await _context.Patients
                .AsNoTracking()
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            return patient?.ToDto();
        }

        public async Task<CrudOperationResult<PatientDto>> CreateAsync(PatientDto dto)
        {
            try
            {
                var entity = dto.ToEntity();
                entity.Id = Guid.NewGuid();

                _context.Patients.Add(entity);
                await _context.SaveChangesAsync();

                var newDto = await GetByIdAsync(entity.Id);

                return new CrudOperationResult<PatientDto>
                {
                    Result = newDto,
                    Status = CrudOperationResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new CrudOperationResult<PatientDto>
                {
                    Status = CrudOperationResultStatus.Failure,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<CrudOperationResult<PatientDto>> UpdateAsync(Guid id, PatientDto dto)
        {
            try
            {
                var existingPatient = await _context.Patients
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (existingPatient == null)
                {
                    return new CrudOperationResult<PatientDto>
                    {
                        Status = CrudOperationResultStatus.RecordNotFound
                    };
                }

                existingPatient.FirstName = dto.FirstName;
                existingPatient.LastName = dto.LastName;
                existingPatient.DateOfBirth = dto.DateOfBirth;

                await _context.SaveChangesAsync();

                var updatedDto = await GetByIdAsync(id);
                return new CrudOperationResult<PatientDto>
                {
                    Result = updatedDto,
                    Status = CrudOperationResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new CrudOperationResult<PatientDto>
                {
                    Status = CrudOperationResultStatus.Failure,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<CrudOperationResult<PatientDto>> DeleteAsync(Guid id)
        {
            try
            {
                var patient = await _context.Patients
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (patient == null)
                {
                    return new CrudOperationResult<PatientDto>
                    {
                        Status = CrudOperationResultStatus.RecordNotFound
                    };
                }

                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();

                return new CrudOperationResult<PatientDto>
                {
                    Status = CrudOperationResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new CrudOperationResult<PatientDto>
                {
                    Status = CrudOperationResultStatus.Failure,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}