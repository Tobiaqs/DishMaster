using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DishMaster.Annotations
{
    public class ContainsGuids : ValidationAttribute
    {
        private readonly int minItems;
        private readonly bool allowNulls;
        private static readonly ValidationResult errorResultInvalidGuid = new ValidationResult("One of the GUIDs was not valid.");
        private static readonly ValidationResult errorResultTooLittleItems = new ValidationResult("Not enough GUIDs.");
        private System.Guid outGuid = System.Guid.Empty;

        public ContainsGuids()
        {
        }

        public ContainsGuids(int minItems, bool allowNulls)
        {
            this.minItems = minItems;
            this.allowNulls = allowNulls;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var strings = value as IEnumerable<string>;
            if (strings == null && minItems == 0)
            {
                return ValidationResult.Success;
            }

            if (strings.Count() < minItems)
            {
                return errorResultTooLittleItems;
            }

            return strings.Any((str) => (str == null && !allowNulls) || (str != null && !System.Guid.TryParse(str, out outGuid))) ? errorResultInvalidGuid : ValidationResult.Success;
        }
    }
}