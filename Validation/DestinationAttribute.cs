using System.ComponentModel.DataAnnotations;
using LinkShortener.Utils;

namespace LinkShortener.Validation
{
    public class DestinationAttribute : ValidationAttribute
    {
        public DestinationAttribute()
        {
            const string defaultErrorMessage = "Error with Destination.";
            ErrorMessage ??= defaultErrorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || value.GetType() != typeof(string))
            {
                return new ValidationResult("Destination must be a string.");
            }

            string url = (string)value;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                url = "http://" + url;
            }

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return new ValidationResult("Must be a valid URL.");
            }

            return ValidationResult.Success;
        }
    }
}
