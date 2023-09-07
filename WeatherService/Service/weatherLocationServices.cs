using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WeatherService.Models;

namespace WeatherService.Service
{
    public class weatherLocationServices
    {

        private readonly IMongoCollection<WeatherLocation> _weatherLocationCollection;

        public weatherLocationServices(
            IOptions<StoreDatabaseSettings> StoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                StoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                StoreDatabaseSettings.Value.DatabaseName);

            _weatherLocationCollection = mongoDatabase.GetCollection<WeatherLocation>(
                StoreDatabaseSettings.Value.LocationCollectionName);
        }

        public async Task<List<WeatherLocation>> GetAsync() =>
            await _weatherLocationCollection.Find(_ => true).ToListAsync();

        public async Task<List<WeatherLocation>> GetAsync(double latitude, double longitude, DateTime dateTime) =>
            await _weatherLocationCollection.Find(x => x.Latitude== latitude && x.Longitude==longitude && x.Generationtime>dateTime).ToListAsync();

        public async Task<WeatherLocation?> GetAsync(string id) =>
            await _weatherLocationCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(WeatherLocation newLocation) =>
            await _weatherLocationCollection.InsertOneAsync(newLocation);

        public async Task UpdateAsync(string id, WeatherLocation updatedLocation) =>
            await _weatherLocationCollection.ReplaceOneAsync(x => x.Id == id, updatedLocation);

        public async Task RemoveAsync(string id) =>
            await _weatherLocationCollection.DeleteOneAsync(x => x.Id == id);

    }
}
