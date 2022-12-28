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
        [DateValidation(ErrorMessage = "Date must be in the future")]
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

    public class DateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var date = value as DateTime?;
            DateTime now = DateTime.Now;
            if (date > now)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Date must be in the future");
        }
    }
}
