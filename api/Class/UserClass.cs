using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Cocktails.Class
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string? IdUser { get; set; }        
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
    }
}
