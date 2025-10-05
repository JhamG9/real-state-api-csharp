using Microsoft.AspNetCore.Mvc;
using RealEstate.API.Models;
using RealEstate.API.Services;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly PropertyService _service;

    public PropertyController(PropertyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? name = null,
        [FromQuery] string? address = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null)
    {
        var filter = new PropertySearchFilter
        {
            Name = name,
            Address = address,
            MinPrice = minPrice,
            MaxPrice = maxPrice
        };
        
        var list = await _service.GetAsync(filter);
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var property = await _service.GetByIdAsync(id);
        if (property == null) return NotFound();
        return Ok(property);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Property property)
    {
        var created = await _service.CreateAsync(property);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] PropertyUpdateDto updateData)
    {
        var updated = await _service.UpdatePartialAsync(id, updateData);
        if (updated == null) return NotFound("Property not found");
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await _service.DeleteAsync(id);
        return NoContent();
    }
}
