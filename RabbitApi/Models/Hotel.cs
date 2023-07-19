using MongoDB.Bson.Serialization.Attributes;

namespace RabbitApi.Models {
    public class Hotel {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public string city { get; set; } = null!;
        public string hotelName { get; set; } = null!;
        public int price { get; set; }
        public DateTime? date { get; set; }
    }
}
