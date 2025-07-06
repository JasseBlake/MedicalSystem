using Microsoft.EntityFrameworkCore;
using MedicalSystem.Data;
using MedicalSystem.DTOs;
using MedicalSystem.Extensions;
using MedicalSystem.Models;

namespace MedicalSystem.Services
{
    public class AppointmentService
    {
        private readonly MedicalDbContext _context;
        private readonly NotificationService _notificationService;  
        private readonly DoctorService _doctorService;              
        private readonly PatientService _patientService;            

        public AppointmentService(MedicalDbContext context,
            NotificationService notificationService,
            DoctorService doctorService,
            PatientService patientService)
        {
            _context = context;
            _notificationService = notificationService;  
            _doctorService = doctorService;
            _patientService = patientService;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAsync()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .AsNoTracking()
                .Select(e => e.ToDto())
                .ToListAsync();
            return appointments;
        }

        public async Task<AppointmentDto?> GetByIdAsync(Guid id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .AsNoTracking()
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            return appointment?.ToDto();
        }

        public async Task<CrudOperationResult<AppointmentDto>> CreateAsync(AppointmentDto dto)
        {
            try
            {
                // czy lekarz istnieje
                var doctor = await _doctorService.GetByIdAsync(dto.DoctorId);
                if (doctor == null)
                {
                    return new CrudOperationResult<AppointmentDto>
                    {
                        Status = CrudOperationResultStatus.Failure,
                        ErrorMessage = "Lekarz nie istnieje"
                    };
                }

                // czy pacjent istnieje
                var patient = await _patientService.GetByIdAsync(dto.PatientId);
                if (patient == null)
                {
                    return new CrudOperationResult<AppointmentDto>
                    {
                        Status = CrudOperationResultStatus.Failure,
                        ErrorMessage = "Pacjent nie istnieje"
                    };
                }

                var entity = dto.ToEntity();
                entity.Id = Guid.NewGuid();

                _context.Appointments.Add(entity);
                await _context.SaveChangesAsync();

                // utwórz powiadomienie
                await _notificationService.CreateAppointmentNotificationAsync(
                    entity.Id,
                    "Nowa wizyta",
                    $"Wizyta u {doctor.FullName} z pacjentem {patient.FullName} na dzień {entity.AppointmentDate:dd.MM.yyyy HH:mm}"
                );

                var newDto = await GetByIdAsync(entity.Id);

                return new CrudOperationResult<AppointmentDto>
                {
                    Result = newDto,
                    Status = CrudOperationResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new CrudOperationResult<AppointmentDto>
                {
                    Status = CrudOperationResultStatus.Failure,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<CrudOperationResult<AppointmentDto>> DeleteAsync(Guid id)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .Where(e => e.Id == id)
                    .SingleOrDefaultAsync();

                if (appointment == null)
                {
                    return new CrudOperationResult<AppointmentDto>
                    {
                        Status = CrudOperationResultStatus.RecordNotFound
                    };
                }

                // utwórz powiadomienie o anulowaniu
               // await _notificationService.CreateAppointmentNotificationAsync(
               //     appointment.Id,
               //     "Wizyta anulowana",
               //     $"Wizyta u {appointment.Doctor.FirstName} {appointment.Doctor.LastName} z pacjentem {appointment.Patient.FirstName} {appointment.Patient.LastName} została anulowana"
               // );

                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();

                return new CrudOperationResult<AppointmentDto>
                {
                    Status = CrudOperationResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new CrudOperationResult<AppointmentDto>
                {
                    Status = CrudOperationResultStatus.Failure,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}