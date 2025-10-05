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
