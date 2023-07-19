using MongoDB.Bson.Serialization.Attributes;

namespace RabbitApi.Models {
    public class Flight {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        public string airline { get; set; } = null!;
        public int airlineid { get; set; }
        public string srcairport { get; set; } = null!;
        public int srcairportid { get; set; }
        public string destairport { get; set; } = null!;
        public int destairportid { get; set; }
        public string codeshare { get; set; } = null!;
        public int stop { get; set; }
        public string eq { get; set; } = null!;
        public string airlinename { get; set; } = null!;
        public string srcairportname { get; set; } = null!;
        public string srccity { get; set; } = null!;
        public string srccountry { get; set; } = null!;
        public string destairportname { get; set; } = null!;
        public string destcity { get; set; } = null!;
        public string destcountry { get; set; } = null!;
        public int price { get; set; }
        public DateTime date { get; set; }
    }
}
