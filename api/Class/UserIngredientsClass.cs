using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cocktails.Class
{
    public class UserIngredients
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]

        public string? _id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdUser { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdIngredients { get; set; }
        public bool? isOwned { get; set; }
    }
}
