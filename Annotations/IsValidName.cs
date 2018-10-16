using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Annotations
{
    public class IsValidName : ValidationAttribute
    {
        private static readonly ValidationResult errorResult = new ValidationResult("The name was not valid.");
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return errorResult;
            }

            var str = value as string;
            
            return str.Length >= 1 && str.Length <= 32 ? ValidationResult.Success : errorResult;
        }
    }
}