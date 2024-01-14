using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace order_service.Models;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Category { get; set; } = null!;

    public string Author { get; set; } = null!;
    public Dictionary<string, int> Products { get; set; } = null!;
    public string Status { get; set; } = "PENDING";
}