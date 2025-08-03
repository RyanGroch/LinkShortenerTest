using System.ComponentModel.DataAnnotations;

namespace SmallUrl.Validation
{
    public class SlugLengthAttribute : StringLengthAttribute
    {
        const int MAX_LEN = 100;
        const int MIN_LEN = 1;

        public SlugLengthAttribute() : base(MAX_LEN)
        {
            MinimumLength = MIN_LEN;
            ErrorMessage = $"Slug must be between {MIN_LEN} and {MAX_LEN} characters";
        }
    }
}
