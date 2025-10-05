using MongoDB.Driver;
using RealEstate.API.Models;

namespace RealEstate.API.Seeders;

public class OwnerSeeder : BaseSeeder
{
    private readonly IMongoCollection<Owner> _owners;

    public OwnerSeeder(IConfiguration config) : base(config)
    {
        _owners = _database.GetCollection<Owner>("Owners");
    }

    public override string SeederName => "Owner";

    public override async Task<bool> ShouldSeedAsync()
    {
        return await _owners.CountDocumentsAsync(_ => true) == 0;
    }

    public override async Task SeedAsync()
    {
        var owners = new List<Owner>
        {
            new Owner
            {
                Name = "Juan Carlos Pérez",
                Address = "Calle 85 #12-34, Bogotá",
                Photo = "https://randomuser.me/api/portraits/men/1.jpg",
                Birthday = "1985-03-15"
            },
            new Owner
            {
                Name = "María Elena González",
                Address = "Carrera 15 #78-90, Medellín",
                Photo = "https://randomuser.me/api/portraits/women/2.jpg",
                Birthday = "1978-11-22"
            },
            new Owner
            {
                Name = "Carlos Alberto Rodríguez",
                Address = "Avenida 6 #45-67, Cali",
                Photo = "https://randomuser.me/api/portraits/men/3.jpg",
                Birthday = "1990-07-08"
            },
            new Owner
            {
                Name = "Ana Sofía Martínez",
                Address = "Calle 72 #23-45, Barranquilla",
                Photo = "https://randomuser.me/api/portraits/women/4.jpg",
                Birthday = "1982-12-03"
            },
            new Owner
            {
                Name = "Diego Fernando López",
                Address = "Carrera 11 #56-78, Bucaramanga",
                Photo = "https://randomuser.me/api/portraits/men/5.jpg",
                Birthday = "1987-09-18"
            }
        };

        await _owners.InsertManyAsync(owners);
    }

    public override async Task ClearAsync()
    {
        await _owners.DeleteManyAsync(_ => true);
    }

    public async Task<List<Owner>> GetOwnersAsync()
    {
        return await _owners.Find(_ => true).ToListAsync();
    }
}