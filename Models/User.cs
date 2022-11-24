using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ToDo_List_with_additions.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("login")]
        [Required]
        public string? Login { get; set; }
        [BsonElement("password")]
        [Required]
        public string? Password { get; set; }
        [BsonElement("firstName")]
        [Required]
        public string? FirstName { get; set; }
        [BsonElement("lastName")]
        [Required]
        public string? LastName { get; set; }
        [BsonElement("nickname")]
        [Required]
        public string? Nickname { get; set; }
    }
}
