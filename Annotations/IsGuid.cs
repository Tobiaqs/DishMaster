using System.ComponentModel.DataAnnotations;

namespace wie_doet_de_afwas.Annotations
{
    public class IsGuid : ValidationAttribute
    {
        private static readonly ValidationResult errorResult = new ValidationResult("The GUID was not valid.");
        private System.Guid outGuid = System.Guid.Empty;
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return errorResult;
            }
            
            return System.Guid.TryParse(value as string, out outGuid) ? ValidationResult.Success : errorResult;
        }
    }
}