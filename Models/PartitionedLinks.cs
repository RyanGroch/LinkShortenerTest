namespace LinkShortener.Models
{
    public class PartitionedLinks
    {
        public required List<ShortLink> UnownedLinks;
        public required List<ShortLink> OwnedLinks;
    }
}
