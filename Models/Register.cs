using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDo_List_with_additions.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nickname { get; set; }
    }
}
