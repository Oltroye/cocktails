using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cocktails.Class
{
    public class Cocktail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string? IdCocktail { get; set; }

        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string? GlassType { get; set; }
        public string? Description { get; set; }
        public bool? AlcoolFree { get; set; }
        public int? Difficulty { get; set; }
        public int? Popularity { get; set; }
        public string? Recipe { get; set; }
        public float? PreparationTime { get; set; }
    }
}
