using Microsoft.AspNetCore.Mvc;
using RealEstate.API.Models;
using RealEstate.API.Services;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyTraceController : ControllerBase
    {
        private readonly PropertyTraceService _service;

        public PropertyTraceController(PropertyTraceService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PropertyTrace>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var propertyTraces = await _service.GetAsync();
            return Ok(propertyTraces);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("ID cannot be null or empty");

            var propertyTrace = await _service.GetByIdAsync(id);
            if (propertyTrace == null)
                return NotFound($"PropertyTrace with ID {id} not found");

            return Ok(propertyTrace);
        }

        [HttpGet("byProperty/{propertyId}")]
        public async Task<IActionResult> GetByProperty(string propertyId)
        {
            if (string.IsNullOrEmpty(propertyId))
                return BadRequest("Property ID cannot be null or empty");

            var propertyTraces = await _service.GetByPropertyAsync(propertyId);
            return Ok(propertyTraces);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PropertyTrace propertyTrace)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdPropertyTrace = await _service.CreateAsync(propertyTrace);
                return CreatedAtAction(nameof(GetById), 
                    new { id = createdPropertyTrace.IdPropertyTrace }, 
                    createdPropertyTrace);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating property trace: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePartial(string id, [FromBody] PropertyTraceUpdateDTO updateDto)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("ID cannot be null or empty");

            if (updateDto == null)
                return BadRequest("Update data cannot be null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedPropertyTrace = await _service.UpdateAsync(id, updateDto);
                if (updatedPropertyTrace == null)
                    return NotFound($"PropertyTrace with ID {id} not found");

                return Ok(updatedPropertyTrace);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating property trace: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Replace(string id, [FromBody] PropertyTrace propertyTrace)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("ID cannot be null or empty");

            if (propertyTrace == null)
                return BadRequest("PropertyTrace data cannot be null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var replacedPropertyTrace = await _service.ReplaceAsync(id, propertyTrace);
                if (replacedPropertyTrace == null)
                    return NotFound($"PropertyTrace with ID {id} not found");

                return Ok(replacedPropertyTrace);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error replacing property trace: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("ID cannot be null or empty");

            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                    return NotFound($"PropertyTrace with ID {id} not found");

                return Ok(new { message = "PropertyTrace deleted successfully", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting property trace: {ex.Message}");
            }
        }

    }
}