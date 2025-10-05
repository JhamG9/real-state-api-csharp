using MongoDB.Driver;
using RealEstate.API.Models;

namespace RealEstate.API.Seeders;

public class PropertySeeder : BaseSeeder
{
    private readonly IMongoCollection<Property> _properties;
    private readonly OwnerSeeder _ownerSeeder;

    public PropertySeeder(IConfiguration config, OwnerSeeder ownerSeeder) : base(config)
    {
        _properties = _database.GetCollection<Property>("Properties");
        _ownerSeeder = ownerSeeder;
    }

    public override string SeederName => "Property";

    public override async Task<bool> ShouldSeedAsync()
    {
        return await _properties.CountDocumentsAsync(_ => true) == 0;
    }

    public override async Task SeedAsync()
    {
        // Obtener owners existentes
        var owners = await _ownerSeeder.GetOwnersAsync();
        if (!owners.Any())
        {
            throw new InvalidOperationException("No hay owners disponibles. Ejecuta OwnerSeeder primero.");
        }

        var properties = new List<Property>
        {
            new Property
            {
                Name = "Casa Centro Histórico",
                Address = "Calle 75 #12-34, Centro, Bogotá",
                Price = 450000000,
                CodeInternal = "BOG-001",
                Year = 2018,
                IdOwner = owners[0].IdOwner!
            },
            new Property
            {
                Name = "Apartamento Zona Rosa",
                Address = "Carrera 14 #85-67, Zona Rosa, Bogotá",
                Price = 320000000,
                CodeInternal = "BOG-002",
                Year = 2020,
                IdOwner = owners[1].IdOwner!
            },
            new Property
            {
                Name = "Casa Campestre El Poblado",
                Address = "Calle 10 #43-21, El Poblado, Medellín",
                Price = 680000000,
                CodeInternal = "MED-001",
                Year = 2019,
                IdOwner = owners[1].IdOwner!
            },
            new Property
            {
                Name = "Apartamento Laureles",
                Address = "Carrera 73 #45-67, Laureles, Medellín",
                Price = 280000000,
                CodeInternal = "MED-002",
                Year = 2021,
                IdOwner = owners[2].IdOwner!
            },
            new Property
            {
                Name = "Casa Granada Norte",
                Address = "Calle 15 #78-90, Granada, Cali",
                Price = 420000000,
                CodeInternal = "CAL-001",
                Year = 2017,
                IdOwner = owners[3].IdOwner!
            },
            new Property
            {
                Name = "Penthouse Ciudad Jardín",
                Address = "Carrera 100 #23-45, Ciudad Jardín, Cali",
                Price = 850000000,
                CodeInternal = "CAL-002",
                Year = 2022,
                IdOwner = owners[4].IdOwner!
            },
            new Property
            {
                Name = "Casa El Prado",
                Address = "Calle 72 #34-56, El Prado, Barranquilla",
                Price = 380000000,
                CodeInternal = "BAQ-001",
                Year = 2016,
                IdOwner = owners[0].IdOwner!
            },
            new Property
            {
                Name = "Apartamento Cabecera",
                Address = "Carrera 35 #56-78, Cabecera, Bucaramanga",
                Price = 250000000,
                CodeInternal = "BUC-001",
                Year = 2020,
                IdOwner = owners[2].IdOwner!
            }
        };

        await _properties.InsertManyAsync(properties);
    }

    public override async Task ClearAsync()
    {
        await _properties.DeleteManyAsync(_ => true);
    }

    public async Task<List<Property>> GetPropertiesAsync()
    {
        return await _properties.Find(_ => true).ToListAsync();
    }
}