using MongoDB.Driver;
using RealEstate.API.Models;

namespace RealEstate.API.Seeders;

public class PropertyImageSeeder : BaseSeeder
{
    private readonly IMongoCollection<PropertyImage> _propertyImages;
    private readonly PropertySeeder _propertySeeder;

    public PropertyImageSeeder(IConfiguration config, PropertySeeder propertySeeder) : base(config)
    {
        _propertyImages = _database.GetCollection<PropertyImage>("PropertyImages");
        _propertySeeder = propertySeeder;
    }

    public override string SeederName => "PropertyImage";

    public override async Task<bool> ShouldSeedAsync()
    {
        return await _propertyImages.CountDocumentsAsync(_ => true) == 0;
    }

    public override async Task SeedAsync()
    {
        var properties = await _propertySeeder.GetPropertiesAsync();
        if (!properties.Any())
        {
            throw new InvalidOperationException("No hay propiedades disponibles. Ejecuta PropertySeeder primero.");
        }

        var images = new List<PropertyImage>();
        var imageUrls = new[]
        {
            "https://images.unsplash.com/photo-1568605114967-8130f3a36994?w=800",
            "https://images.unsplash.com/photo-1570129477492-45c003edd2be?w=800", 
            "https://images.unsplash.com/photo-1560518883-ce09059eeffa?w=800",
            "https://images.unsplash.com/photo-1582268611958-ebfd161ef9cf?w=800",
            "https://images.unsplash.com/photo-1574362848149-11496d93a7c7?w=800"
        };

        foreach (var property in properties)
        {
            // Agregar 2-3 imágenes por propiedad
            var imageCount = new Random().Next(2, 4);
            for (int i = 0; i < imageCount; i++)
            {
                var randomUrl = imageUrls[new Random().Next(imageUrls.Length)];
                images.Add(new PropertyImage
                {
                    IdProperty = property.IdProperty!,
                    FilePath = randomUrl,
                    Enabled = true
                });
            }
        }

        await _propertyImages.InsertManyAsync(images);
    }

    public override async Task ClearAsync()
    {
        await _propertyImages.DeleteManyAsync(_ => true);
    }
}