using MongoDB.Driver;
using RealEstate.API.Models;

namespace RealEstate.API.Services;

public class OwnerService
{
    private readonly IMongoCollection<Owner> _owners;

    public OwnerService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoSettings:ConnectionString"]);
        var database = client.GetDatabase(config["MongoSettings:DatabaseName"]);
        _owners = database.GetCollection<Owner>("Owners"); // nombre de la colección
    }

    public async Task<List<Owner>> GetAsync() =>
        await _owners.Find(_ => true).ToListAsync();

    public async Task<Owner?> GetByIdAsync(string id) =>
        await _owners.Find(o => o.IdOwner == id).FirstOrDefaultAsync();

    public async Task<Owner> CreateAsync(Owner owner)
    {
        await _owners.InsertOneAsync(owner);
        return owner;
    }

    public async Task UpdateAsync(string id, Owner updatedOwner) =>
        await _owners.ReplaceOneAsync(o => o.IdOwner == id, updatedOwner);

    public async Task DeleteAsync(string id) =>
        await _owners.DeleteOneAsync(o => o.IdOwner == id);
}
