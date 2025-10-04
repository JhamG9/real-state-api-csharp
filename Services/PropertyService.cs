using MongoDB.Driver;
using RealEstate.API.Models;

namespace RealEstate.API.Services;

public class PropertyService
{
    private readonly IMongoCollection<Property> _properties;

    public PropertyService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoSettings:ConnectionString"]);
        var database = client.GetDatabase(config["MongoSettings:DatabaseName"]);
        _properties = database.GetCollection<Property>("Properties");
    }

    public async Task<List<Property>> GetAsync() =>
        await _properties.Find(_ => true).ToListAsync();

    public async Task<Property?> GetByIdAsync(string id) =>
        await _properties.Find(p => p.IdProperty == id).FirstOrDefaultAsync();

    public async Task<Property> CreateAsync(Property property)
    {
        await _properties.InsertOneAsync(property);
        return property;
    }

    public async Task<Property?> UpdatePartialAsync(string id, PropertyUpdateDto updateData)
    {
        var property = await GetByIdAsync(id);
        if (property == null) return null;

        property.Name = updateData.Name ?? property.Name;
        property.Address = updateData.Address ?? property.Address;
        property.Price = updateData.Price ?? property.Price;
        property.CodeInternal = updateData.CodeInternal ?? property.CodeInternal;
        property.Year = updateData.Year ?? property.Year;
        property.IdOwner = updateData.IdOwner ?? property.IdOwner;

        await _properties.ReplaceOneAsync(p => p.IdProperty == id, property);
        return property;
    }

    public async Task DeleteAsync(string id) =>
        await _properties.DeleteOneAsync(p => p.IdProperty == id);
}
