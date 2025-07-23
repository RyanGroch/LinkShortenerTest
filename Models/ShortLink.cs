using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LinkShortener.Validation;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Models
{
    [Index(nameof(Slug), IsUnique = true)]
    public class ShortLink
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ShortLinkId { get; set; }

        [Required]
        [Destination]
        public required string Destination { get; set; }

        [Required]
        [SlugLength]
        [Slug]
        public required string Slug { get; set; }

        public string? OwnerId { get; set; }
    }
}
