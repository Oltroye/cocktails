using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cocktails.Class
{
    public class UserChoice
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string? _id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdUser {  get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdCocktail { get; set; }
    }
}
