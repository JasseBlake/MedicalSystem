using Microsoft.AspNetCore.Mvc;
using MedicalSystem.DTOs;
using MedicalSystem.Services;

namespace MedicalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorService _doctorService;

        public DoctorController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IEnumerable<DoctorDto>> GetDoctors()
        {
            return await _doctorService.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(Guid id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null)
                return NotFound();

            return Ok(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _doctorService.CreateAsync(dto);

            if (result.Status == CrudOperationResultStatus.Success)
                return Ok(result.Result);

            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(Guid id, [FromBody] DoctorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _doctorService.UpdateAsync(id, dto);

            return result.Status switch
            {
                CrudOperationResultStatus.Success => Ok(result.Result),
                CrudOperationResultStatus.RecordNotFound => NotFound(),
                _ => BadRequest(result.ErrorMessage)
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            var result = await _doctorService.DeleteAsync(id);

            return result.Status switch
            {
                CrudOperationResultStatus.Success => NoContent(),
                CrudOperationResultStatus.RecordNotFound => NotFound(),
                _ => BadRequest(result.ErrorMessage)
            };
        }
    }
}