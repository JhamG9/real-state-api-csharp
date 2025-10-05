using MongoDB.Driver;
using RealEstate.API.Models;

namespace RealEstate.API.Services;

public class PropertyService
{
    private readonly IMongoCollection<Property> _properties;
    private readonly IMongoCollection<PropertyImage> _images;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly OwnerService _ownerService;
    private readonly PropertyTraceService _propertyTraceService;

    public PropertyService(IConfiguration config, IHttpContextAccessor httpContextAccessor, OwnerService ownerService, PropertyTraceService propertyTraceService)
    {
        var client = new MongoClient(config["MongoSettings:ConnectionString"]);
        var database = client.GetDatabase(config["MongoSettings:DatabaseName"]);
        _properties = database.GetCollection<Property>("Properties");
        _images = database.GetCollection<PropertyImage>("PropertyImages");
        _httpContextAccessor = httpContextAccessor;
        _ownerService = ownerService;
        _propertyTraceService = propertyTraceService;
    }

    public async Task<List<Property>> GetAsync(PropertySearchFilter? filter = null)
    {
        var filterBuilder = Builders<Property>.Filter;
        var mongoFilter = filterBuilder.Empty;

        if (filter != null)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                mongoFilter &= filterBuilder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(filter.Name, "i"));
            }

            if (!string.IsNullOrEmpty(filter.Address))
            {
                mongoFilter &= filterBuilder.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(filter.Address, "i"));
            }

            if (filter.MinPrice.HasValue)
            {
                mongoFilter &= filterBuilder.Gte(p => p.Price, filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                mongoFilter &= filterBuilder.Lte(p => p.Price, filter.MaxPrice.Value);
            }
        }

        var properties = await _properties.Find(mongoFilter).ToListAsync();
        foreach (var property in properties)
        {
            await LoadPropertyImagesAsync(property);
            await LoadPropertyOwnerAsync(property);
            await LoadPropertyTracesAsync(property);
        }

        return properties;
    }

    public async Task<Property?> GetByIdAsync(string id)
    {
        var property = await _properties.Find(p => p.IdProperty == id).FirstOrDefaultAsync();
        if (property != null)
        {
            await LoadPropertyImagesAsync(property);
            await LoadPropertyOwnerAsync(property);
            await LoadPropertyTracesAsync(property);
        }
        return property;
    }

    public async Task<Property> CreateAsync(Property property)
    {
        await _properties.InsertOneAsync(property);
        return property;
    }

    public async Task<Property?> UpdatePartialAsync(string id, PropertyUpdateDto updateData)
    {
        var property = await _properties.Find(p => p.IdProperty == id).FirstOrDefaultAsync();
        if (property == null) return null;

        property.Name = updateData.Name ?? property.Name;
        property.Address = updateData.Address ?? property.Address;
        property.Price = updateData.Price ?? property.Price;
        property.CodeInternal = updateData.CodeInternal ?? property.CodeInternal;
        property.Year = updateData.Year ?? property.Year;
        property.IdOwner = updateData.IdOwner ?? property.IdOwner;

        await _properties.ReplaceOneAsync(p => p.IdProperty == id, property);

        await LoadPropertyImagesAsync(property);
        await LoadPropertyOwnerAsync(property);
        await LoadPropertyTracesAsync(property);
        return property;
    }

    public async Task DeleteAsync(string id) =>
        await _properties.DeleteOneAsync(p => p.IdProperty == id);

    private async Task LoadPropertyImagesAsync(Property property)
    {
        if (property?.IdProperty == null) return;

        var propertyImages = await _images
            .Find(img => img.IdProperty == property.IdProperty && img.Enabled)
            .ToListAsync();

        var baseUrl = GetBaseUrl();
        property.Images = propertyImages
            .Select(img => $"{baseUrl}{img.FilePath}")
            .ToList();
    }

    private async Task LoadPropertyOwnerAsync(Property property)
    {
        if (property?.IdOwner == null) return;

        property.Owner = await _ownerService.GetByIdAsync(property.IdOwner);
    }

    private async Task LoadPropertyTracesAsync(Property property)
    {
        if (property?.IdProperty == null) return;

        property.PropertyTraces = await _propertyTraceService.GetByPropertyAsync(property.IdProperty);
    }

    private string GetBaseUrl()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
        {
            return "http://localhost:5189";
        }

        var scheme = request.Scheme;
        var host = request.Host.Value;
        return $"{scheme}://{host}";
    }
}
