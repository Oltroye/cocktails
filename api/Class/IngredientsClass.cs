using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cocktails.Class
{
    public class Ingredients
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string? IdIngredients { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdType { get; set; } = string.Empty;
        public string? Name { get; set; }
    }
}
