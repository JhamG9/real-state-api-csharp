using MongoDB.Driver;
using RealEstate.API.Models;

namespace RealEstate.API.Seeders;

public class PropertyTraceSeeder : BaseSeeder
{
    private readonly IMongoCollection<PropertyTrace> _propertyTraces;
    private readonly PropertySeeder _propertySeeder;

    public PropertyTraceSeeder(IConfiguration config, PropertySeeder propertySeeder) : base(config)
    {
        _propertyTraces = _database.GetCollection<PropertyTrace>("PropertyTraces");
        _propertySeeder = propertySeeder;
    }

    public override string SeederName => "PropertyTrace";

    public override async Task<bool> ShouldSeedAsync()
    {
        return await _propertyTraces.CountDocumentsAsync(_ => true) == 0;
    }

    public override async Task SeedAsync()
    {
        var properties = await _propertySeeder.GetPropertiesAsync();
        if (!properties.Any())
        {
            throw new InvalidOperationException("No hay propiedades disponibles. Ejecuta PropertySeeder primero.");
        }

        var traces = new List<PropertyTrace>();
        var traceNames = new[]
        {
            "Venta Inicial",
            "Remodelación Cocina", 
            "Reparación Techos",
            "Ampliación Sala",
            "Renovación Baños",
            "Instalación Aires",
            "Pintura General",
            "Cambio Pisos"
        };

        foreach (var property in properties)
        {
            // Agregar 2-4 traces por propiedad
            var traceCount = new Random().Next(2, 5);
            var baseDate = DateTime.Now.AddMonths(-24); // Empezar hace 2 años

            for (int i = 0; i < traceCount; i++)
            {
                var randomName = traceNames[new Random().Next(traceNames.Length)];
                var value = new Random().Next(5000000, 50000000); // Entre 5M y 50M
                var tax = value * 0.1m; // 10% de impuesto

                traces.Add(new PropertyTrace
                {
                    IdProperty = property.IdProperty!,
                    DateSale = baseDate.AddMonths(i * 6), // Cada 6 meses
                    Name = randomName,
                    Value = value,
                    Tax = tax
                });
            }
        }

        await _propertyTraces.InsertManyAsync(traces);
    }

    public override async Task ClearAsync()
    {
        await _propertyTraces.DeleteManyAsync(_ => true);
    }
}