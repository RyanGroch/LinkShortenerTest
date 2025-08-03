using System.ComponentModel.DataAnnotations;
using SmallUrl.Validation;

namespace SmallUrl.Models
{
    public class ShortLinkView
    {
        public Guid? ShortLinkId { get; set; }

        [Required(ErrorMessage = "Full URL is required.")]
        [Destination]
        public required string Destination { get; set; }

        [Required(ErrorMessage = "An alias for your URL is required.")]
        [SlugLength]
        [Slug]
        public required string Slug { get; set; }
    }
}
