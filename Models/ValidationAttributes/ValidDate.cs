using System.ComponentModel.DataAnnotations;


namespace ToDo_List_with_additions.Models.ValidationAttributes
{

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
        public sealed class ValidDate : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var date = (DateTime)value;
                if (date > DateTime.Now)
                {
                    return new ValidationResult("Date must be in the past");
                }
                return ValidationResult.Success;
            }
        }
    
}
