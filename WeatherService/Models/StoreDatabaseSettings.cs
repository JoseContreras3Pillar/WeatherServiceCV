namespace WeatherService.Models
{
    public class StoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string LocationCollectionName { get; set; } = null!;

        public string CityCollectionName { get; set; } = null!;
    }
}
