namespace RabbitApi.Models {
    public class MongoDBSettings {
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string FlightCollectionName { get; set; } = null!;
        public string HotelCollectionName { get; set; } = null!;
    }
}
