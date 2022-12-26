using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ToDo_List_with_additions.Models
{
    public class ToDoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string UserId { get; set; }
        [BsonElement("date")]
        [Required]
        public DateTime Date { get; set; }
        [BsonElement("content")]
        [Required]
        public string Content { get; set; }
        [BsonElement("importance")]
        [Required]
        public int Importance { get; set; }
        [BsonElement("done")]
        [Required]
        public bool Done { get; set; }
    }
}
