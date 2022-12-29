using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using ToDo_List_with_additions.Models.ValidationAttributes;
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
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
        
        [BsonElement("content")]
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
        
        [BsonElement("importance")]
        [Required(ErrorMessage = "Importance is required")]
        public int Importance { get; set; }
        
        [BsonElement("done")]
        [Required]
        public bool Done { get; set; }

 
       
        
    }

   
}
