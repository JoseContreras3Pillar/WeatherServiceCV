using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WeatherService.Models;

namespace WeatherService.Service
{
    public class weatherCityServices
    {

        private readonly IMongoCollection<WeatherCity> _weatherCityCollection;

        public weatherCityServices(
            IOptions<StoreDatabaseSettings> StoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                StoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                StoreDatabaseSettings.Value.DatabaseName);

            _weatherCityCollection = mongoDatabase.GetCollection<WeatherCity>(
                StoreDatabaseSettings.Value.CityCollectionName);
        }

        public async Task<List<WeatherCity>> GetAsync() =>
            await _weatherCityCollection.Find(_ => true).ToListAsync();

        public async Task<List<WeatherCity>> GetAsync(string cityName) =>
            await _weatherCityCollection.Find(x => x.Name.ToUpper() == cityName).ToListAsync();

        public async Task CreateAsync(WeatherCity newCity) =>
            await _weatherCityCollection.InsertOneAsync(newCity);

        public async Task UpdateAsync(string id, WeatherCity updatedLocation) =>
            await _weatherCityCollection.ReplaceOneAsync(x => x.Id == id, updatedLocation);

        public async Task RemoveAsync(string id) =>
            await _weatherCityCollection.DeleteOneAsync(x => x.Id == id);

    }
}
