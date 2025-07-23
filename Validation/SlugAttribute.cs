using System.ComponentModel.DataAnnotations;
using LinkShortener.Data;
using LinkShortener.Models;

namespace LinkShortener.Validation
{
    public class SlugAttribute : ValidationAttribute
    {
        public SlugAttribute()
        {
            const string defaultErrorMessage = "Error with Slug";
            ErrorMessage ??= defaultErrorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || value.GetType() != typeof(string))
            {
                return new ValidationResult("Slug must be a string.");
            }

            string slug = (string)value;

            if (!slug.All(char.IsLetterOrDigit))
            {
                return new ValidationResult("Must be alphanumeric.");
            }

            string lowercaseSlug = slug.ToLower();

            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext))!;

            var existingShortLink = context.ShortLink.FirstOrDefault(l => l.Slug.Equals(lowercaseSlug));
            var currentShortLinkId = (ShortLinkView) validationContext.ObjectInstance;

            if (existingShortLink != null && existingShortLink.ShortLinkId != currentShortLinkId.ShortLinkId)
            {
                return new ValidationResult("Slug is already taken.");
            }

            return ValidationResult.Success;
        }
    }
}
