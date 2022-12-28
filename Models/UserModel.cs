using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ToDo_List_with_additions.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("login")]
        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }
        
        [BsonElement("password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        
        public string Password { get; set; }
        
        [BsonElement("firstName")]
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }
        
        [BsonElement("lastName")]
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }
        
        [BsonElement("nickname")]
        public string? Nickname { get; set; }
    }
}
