namespace RealEstate.API.Seeders;

public class DatabaseSeeder
{
    private readonly OwnerSeeder _ownerSeeder;
    private readonly PropertySeeder _propertySeeder;
    private readonly PropertyImageSeeder _propertyImageSeeder;
    private readonly PropertyTraceSeeder _propertyTraceSeeder;

    public DatabaseSeeder(IConfiguration config)
    {
        _ownerSeeder = new OwnerSeeder(config);
        _propertySeeder = new PropertySeeder(config, _ownerSeeder);
        _propertyImageSeeder = new PropertyImageSeeder(config, _propertySeeder);
        _propertyTraceSeeder = new PropertyTraceSeeder(config, _propertySeeder);
    }

    public async Task SeedAsync()
    {
        // Verificar si ya hay datos
        if (!await _ownerSeeder.ShouldSeedAsync())
        {
            Console.WriteLine("🌱 Base de datos ya tiene datos. Saltando seeding...");
            return;
        }

        Console.WriteLine("🌱 Iniciando seeding modular de datos...");

        // 1. Owners (prerequisito para todo)
        await SeedEntityAsync(_ownerSeeder);

        // 2. Properties (depende de Owners)
        await SeedEntityAsync(_propertySeeder);

        // 3. PropertyImages (depende de Properties)
        await SeedEntityAsync(_propertyImageSeeder);

        // 4. PropertyTraces (depende de Properties)
        await SeedEntityAsync(_propertyTraceSeeder);

        Console.WriteLine("🎉 ¡Seeding modular completado exitosamente!");
    }

    public async Task ClearDatabaseAsync()
    {
        Console.WriteLine("🗑️ Limpiando base de datos modular...");

        // Orden inverso para respetar dependencias
        await ClearEntityAsync(_propertyTraceSeeder);
        await ClearEntityAsync(_propertyImageSeeder);
        await ClearEntityAsync(_propertySeeder);
        await ClearEntityAsync(_ownerSeeder);

        Console.WriteLine("✅ Base de datos limpiada");
    }

    private async Task SeedEntityAsync(BaseSeeder seeder)
    {
        try
        {
            if (await seeder.ShouldSeedAsync())
            {
                await seeder.SeedAsync();
                Console.WriteLine($"✅ {seeder.SeederName} seeding completado");
            }
            else
            {
                Console.WriteLine($"⏭️ {seeder.SeederName} ya tiene datos, saltando...");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error en {seeder.SeederName}: {ex.Message}");
            throw;
        }
    }

    private async Task ClearEntityAsync(BaseSeeder seeder)
    {
        try
        {
            await seeder.ClearAsync();
            Console.WriteLine($"🗑️ {seeder.SeederName} limpiado");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error limpiando {seeder.SeederName}: {ex.Message}");
            throw;
        }
    }
}