using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cocktails.Class
{
    public class UserChoice
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdUser {  get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdCocktail { get; set; }
    }
}
