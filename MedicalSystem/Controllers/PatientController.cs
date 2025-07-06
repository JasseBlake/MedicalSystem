using Microsoft.AspNetCore.Mvc;
using MedicalSystem.DTOs;
using MedicalSystem.Services;

namespace MedicalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IEnumerable<PatientDto>> GetPatients()
        {
            return await _patientService.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(Guid id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null)
                return NotFound();

            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] PatientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _patientService.CreateAsync(dto);

            if (result.Status == CrudOperationResultStatus.Success)
                return Ok(result.Result);

            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] PatientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _patientService.UpdateAsync(id, dto);

            return result.Status switch
            {
                CrudOperationResultStatus.Success => Ok(result.Result),
                CrudOperationResultStatus.RecordNotFound => NotFound(),
                _ => BadRequest(result.ErrorMessage)
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var result = await _patientService.DeleteAsync(id);

            return result.Status switch
            {
                CrudOperationResultStatus.Success => NoContent(),
                CrudOperationResultStatus.RecordNotFound => NotFound(),
                _ => BadRequest(result.ErrorMessage)
            };
        }
    }
}