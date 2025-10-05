using MongoDB.Driver;

namespace RealEstate.API.Seeders;

public abstract class BaseSeeder
{
    protected readonly IMongoDatabase _database;
    protected readonly IConfiguration _config;

    protected BaseSeeder(IConfiguration config)
    {
        _config = config;
        var client = new MongoClient(config["MongoSettings:ConnectionString"]);
        _database = client.GetDatabase(config["MongoSettings:DatabaseName"]);
    }

    public abstract Task<bool> ShouldSeedAsync();
    public abstract Task SeedAsync();
    public abstract Task ClearAsync();
    public abstract string SeederName { get; }
}