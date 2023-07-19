using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using RabbitApi.Models;
using System.Globalization;

namespace RabbitApi.Services {
    public class MongoDBService {
        private readonly IMongoCollection<Flight> _flightCollection;
        private readonly IMongoCollection<Hotel> _hotelCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
            // Connect to MongoDB
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            _flightCollection = database.GetCollection<Flight>(mongoDBSettings.Value.FlightCollectionName);
            _hotelCollection = database.GetCollection<Hotel>(mongoDBSettings.Value.HotelCollectionName);
        }

        //public async Task<List<Flight>> GetAllFlights() { 
        //    return await _flightCollection.Find(_ => true).ToListAsync();
        //}

        public async Task<List<Flight>> GetDepartureFlights(string departureDate, string destination) {
            DateTime date = DateTime.ParseExact(departureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string departureDateIsoFormat = date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            var filterBuilder = Builders<Flight>.Filter;
            var srccityFilter = filterBuilder.Regex("srccity", new BsonRegularExpression("singapore", "i"));
            var destcityFilter = filterBuilder.Regex("destcity", new BsonRegularExpression(destination, "i"));
            var dateFilter = filterBuilder.Eq("date", departureDateIsoFormat);

            var filter = filterBuilder.And(srccityFilter, destcityFilter, dateFilter);

            var result = await _flightCollection.Find(filter).ToListAsync();
            result = result.OrderBy(i => i.price).ToList();
            return result;
        }

        public async Task<List<Flight>> GetReturnFlights(string returnDate, string destination) {
            DateTime date = DateTime.ParseExact(returnDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string returnDateIsoFormat = date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            var filterBuilder = Builders<Flight>.Filter;
            var srccityFilter = filterBuilder.Regex("srccity", new BsonRegularExpression(destination, "i"));
            var destcityFilter = filterBuilder.Regex("destcity", new BsonRegularExpression("singapore", "i"));
            var dateFilter = filterBuilder.Eq("date", returnDateIsoFormat);

            var filter = filterBuilder.And(srccityFilter, destcityFilter, dateFilter);

            var result = await _flightCollection.Find(filter).ToListAsync();
            result = result.OrderBy(i => i.price).ToList();
            return result;
        }

        public async Task<List<Hotel>> GetCheapestHotels(string checkInDateStr, string checkOutDateStr, string destination) {
            DateTime checkInDate = DateTime.ParseExact(checkInDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime checkOutDate = DateTime.ParseExact(checkOutDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var filter = Builders<Hotel>.Filter.And(
                Builders<Hotel>.Filter.Regex("city", new BsonRegularExpression(destination, "i")),
                Builders<Hotel>.Filter.Gte("date", checkInDate.ToUniversalTime()),
                  Builders<Hotel>.Filter.Lte("date", checkOutDate.ToUniversalTime())
            );

            var filteredResult = await _hotelCollection.Find(filter).ToListAsync();

            return filteredResult;
        }
    }
}
