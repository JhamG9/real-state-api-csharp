using Microsoft.AspNetCore.Mvc;
using RealEstate.API.Models;
using RealEstate.API.Services;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyImageController : ControllerBase
    {
        private readonly PropertyImageService _service;
        private readonly IWebHostEnvironment _env;

        public PropertyImageController(PropertyImageService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var image = await _service.GetByIdAsync(id);
            if (image == null) return NotFound();
            return Ok(image);
        }

        [HttpGet("byProperty/{idProperty}")]
        public async Task<IActionResult> GetByProperty(string idProperty)
        {
            var images = await _service.GetByPropertyAsync(idProperty);

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var result = images.Select(img => new
            {
                img.IdPropertyImage,
                img.IdProperty,
                File = $"{baseUrl}{img.FilePath}",
                img.Enabled
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] string idProperty, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsFolder = Path.Combine("uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var image = new PropertyImage
            {
                IdProperty = idProperty,
                FilePath = $"/public/uploads/{fileName}",
                Enabled = true
            };

            await _service.CreateAsync(image);
            return Ok(image);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return Ok("Image deleted");
        }
    }
}
