// Verificar si se está ejecutando seeding desde línea de comandos
if (args.Length > 0 && args[0] == "seed")
{
    await RunSeedingAsync(args);
    return;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

// Agregar HttpContextAccessor para acceder al contexto HTTP en los servicios
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<RealEstate.API.Services.PropertyService>();
builder.Services.AddSingleton<RealEstate.API.Services.OwnerService>();
builder.Services.AddSingleton<RealEstate.API.Services.PropertyImageService>();
builder.Services.AddSingleton<RealEstate.API.Services.PropertyTraceService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Configurar archivos estáticos para servir imágenes
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/public/uploads"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Json(new { ok = true }));

app.Run();

// Función para ejecutar seeding desde línea de comandos
static async Task RunSeedingAsync(string[] args)
{
    Console.WriteLine("🌱 Ejecutando seeding desde línea de comandos...");
    
    var builder = WebApplication.CreateBuilder(args);
    
    // Configurar servicios necesarios para seeding
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSingleton<RealEstate.API.Services.OwnerService>();
    builder.Services.AddSingleton<RealEstate.API.Services.PropertyService>();
    builder.Services.AddSingleton<RealEstate.API.Services.PropertyImageService>();
    builder.Services.AddSingleton<RealEstate.API.Services.PropertyTraceService>();
    
    var app = builder.Build();
    
    using (var scope = app.Services.CreateScope())
    {
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var seeder = new RealEstate.API.Seeders.DatabaseSeeder(config);
        
        if (args.Length > 1)
        {
            switch (args[1].ToLower())
            {
                case "run":
                    Console.WriteLine("🌱 Ejecutando seeding...");
                    await seeder.SeedAsync();
                    break;
                case "clear":
                    Console.WriteLine("🗑️ Limpiando base de datos...");
                    await seeder.ClearDatabaseAsync();
                    break;
                case "reset":
                    Console.WriteLine("🔄 Reiniciando base de datos...");
                    await seeder.ClearDatabaseAsync();
                    await seeder.SeedAsync();
                    break;
                default:
                    Console.WriteLine("❌ Comando no reconocido. Usa: run, clear, o reset");
                    Console.WriteLine("Ejemplos:");
                    Console.WriteLine("  dotnet run seed run     - Ejecutar seeding");
                    Console.WriteLine("  dotnet run seed clear   - Limpiar BD");
                    Console.WriteLine("  dotnet run seed reset   - Reiniciar BD");
                    break;
            }
        }
        else
        {
            // Por defecto ejecutar seeding
            Console.WriteLine("🌱 Ejecutando seeding por defecto...");
            await seeder.SeedAsync();
        }
    }
    
    Console.WriteLine("✅ Operación completada!");
}
