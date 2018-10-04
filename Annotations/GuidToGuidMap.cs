using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace wie_doet_de_afwas.Annotations
{
    public class GuidToGuidMap : ValidationAttribute
    {
        private readonly int minItems;

        private static readonly ValidationResult errorResultInvalidGuid = new ValidationResult("One of the GUIDs was not valid.");
        private static readonly ValidationResult errorResultTooLittleItems = new ValidationResult("Not enough GUIDs.");
        private System.Guid outGuid = System.Guid.Empty;

        public GuidToGuidMap()
        {
        }

        public GuidToGuidMap(int minItems)
        {
            this.minItems = minItems;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var strings = value as IDictionary<string, string>;
            if (strings == null && minItems == 0)
            {
                return ValidationResult.Success;
            }

            if (strings.Count() < minItems)
            {
                return errorResultTooLittleItems;
            }

            return strings.Any((kv) =>
                !System.Guid.TryParse(kv.Key, out outGuid) ||
                !System.Guid.TryParse(kv.Value, out outGuid))
            ? errorResultInvalidGuid
            : ValidationResult.Success;
        }
    }
}