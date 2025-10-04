using MongoDB.Driver;
using RealEstate.API.Models;

namespace RealEstate.API.Services;

public class PropertyTraceService
{
    private readonly IMongoCollection<PropertyTrace> _propertyTraces;

    public PropertyTraceService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoSettings:ConnectionString"]);
        var database = client.GetDatabase(config["MongoSettings:DatabaseName"]);
        _propertyTraces = database.GetCollection<PropertyTrace>("PropertyTraces");
    }

    public async Task<List<PropertyTrace>> GetAsync() =>
        await _propertyTraces.Find(_ => true).ToListAsync();

    public async Task<PropertyTrace?> GetByIdAsync(string id) =>
        await _propertyTraces.Find(pt => pt.IdPropertyTrace == id).FirstOrDefaultAsync();

    public async Task<List<PropertyTrace>> GetByPropertyAsync(string propertyId) =>
        await _propertyTraces.Find(pt => pt.IdProperty == propertyId).ToListAsync();

    public async Task<PropertyTrace> CreateAsync(PropertyTrace propertyTrace)
    {
        await _propertyTraces.InsertOneAsync(propertyTrace);
        return propertyTrace;
    }

    public async Task<PropertyTrace?> UpdateAsync(string id, PropertyTraceUpdateDTO updateDto)
    {
        var propertyTrace = await GetByIdAsync(id);
        if (propertyTrace == null) return null;

        // Aplicamos solo los cambios que vienen en el body
        propertyTrace.DateSale = updateDto.DateSale ?? propertyTrace.DateSale;
        propertyTrace.Name = updateDto.Name ?? propertyTrace.Name;
        propertyTrace.Value = updateDto.Value ?? propertyTrace.Value;
        propertyTrace.Tax = updateDto.Tax ?? propertyTrace.Tax;
        propertyTrace.IdProperty = updateDto.IdProperty ?? propertyTrace.IdProperty;

        await _propertyTraces.ReplaceOneAsync(pt => pt.IdPropertyTrace == id, propertyTrace);
        return propertyTrace;
    }

    public async Task<PropertyTrace?> ReplaceAsync(string id, PropertyTrace propertyTrace)
    {
        var existingPropertyTrace = await GetByIdAsync(id);
        if (existingPropertyTrace == null) return null;

        propertyTrace.IdPropertyTrace = id; // Mantener el ID original
        await _propertyTraces.ReplaceOneAsync(pt => pt.IdPropertyTrace == id, propertyTrace);
        return propertyTrace;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _propertyTraces.DeleteOneAsync(pt => pt.IdPropertyTrace == id);
        return result.DeletedCount > 0;
    }
}