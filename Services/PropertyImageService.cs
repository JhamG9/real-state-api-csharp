using MongoDB.Driver;
using RealEstate.API.Models;

namespace RealEstate.API.Services;

public class PropertyImageService
{
    private readonly IMongoCollection<PropertyImage> _images;

    public PropertyImageService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoSettings:ConnectionString"]);
        var database = client.GetDatabase(config["MongoSettings:DatabaseName"]);
        _images = database.GetCollection<PropertyImage>("PropertyImages"); // colección de imágenes
    }

    public async Task<List<PropertyImage>> GetAsync() =>
        await _images.Find(_ => true).ToListAsync();

    public async Task<PropertyImage?> GetByIdAsync(string id) =>
        await _images.Find(img => img.IdProperty == id).FirstOrDefaultAsync();

    public async Task<PropertyImage> CreateAsync(PropertyImage image)
    {
        await _images.InsertOneAsync(image);
        return image;
    }

    public async Task DeleteAsync(string id) =>
        await _images.DeleteOneAsync(img => img.IdPropertyImage == id);

    public async Task<List<PropertyImage>> GetByPropertyAsync(string propertyId) =>
        await _images.Find(img => img.IdProperty == propertyId).ToListAsync();
}
