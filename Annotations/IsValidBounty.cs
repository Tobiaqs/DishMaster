using System.ComponentModel.DataAnnotations;

namespace DishMaster.Annotations
{
    public class IsValidBounty : ValidationAttribute
    {
        private static readonly ValidationResult errorResult = new ValidationResult("The bounty was not valid.");
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return errorResult;
            }

            int i = (int) value;

            return i >= 0 && i <= 10 ? ValidationResult.Success : errorResult;
        }
    }
}