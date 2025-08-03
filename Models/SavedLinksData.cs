namespace SmallUrl.Models
{
    public class SavedLinksData
    {
        public required List<ShortLink> UnownedLinks { get; set; }
        public required List<ShortLink> OwnedLinks { get; set; }
        public required string HostUrl { get; set; }
    }
}
