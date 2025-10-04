using Microsoft.AspNetCore.Mvc;
using RealEstate.API.Models;
using RealEstate.API.Services;

namespace RealEstate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnerController : ControllerBase
{
    private readonly OwnerService _service;

    public OwnerController(OwnerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await _service.GetAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var owner = await _service.GetByIdAsync(id);
        if (owner == null) return NotFound();
        return Ok(owner);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Owner owner)
    {
        var created = await _service.CreateAsync(owner);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] OwnerUpdateDto updateData)
    {
        var existingOwner = await _service.GetByIdAsync(id);
        if (existingOwner == null)
            return NotFound("Owner not found");

        existingOwner.Name = updateData.Name ?? existingOwner.Name;
        existingOwner.Address = updateData.Address ?? existingOwner.Address;
        existingOwner.Photo = updateData.Photo ?? existingOwner.Photo;
        existingOwner.Birthday = updateData.Birthday != default
            ? updateData.Birthday
            : existingOwner.Birthday;

        await _service.UpdateAsync(id, existingOwner);

        return Ok(existingOwner);
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
