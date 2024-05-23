using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MoneyAccountingAPIMongoDB.Models
{
    public class Income
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public decimal Sum { get; set; }
        public DateTime Date { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string AccountId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
        public string Source { get; set; }
    }
}
