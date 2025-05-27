using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cocktails.Class
{
    public class CocktailIngredients
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string ? _id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdCocktail {  get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? idIngredient { get; set; }
        public int? Quantity { get; set; }
        public string? Units { get; set; }
    }
}
