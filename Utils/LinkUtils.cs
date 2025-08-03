using SmallUrl.Models;

namespace SmallUrl.Utils
{
    public static class LinkUtils
    {
        public static string CleanDestination(string destination)
        {
            string cleaned = destination;

            if (!Uri.IsWellFormedUriString(destination, UriKind.Absolute))
                cleaned = "http://" + destination;

            return cleaned;
        }

        public static string CleanSlug(string slug)
        {
            return slug.ToLower();
        }

        public static bool UserCanAccessLink(string? userId, ShortLink link, List<Guid> savedLinks)
        {
            return savedLinks.Contains(link.ShortLinkId) && link.OwnerId == null
                    || userId != null && userId == link.OwnerId;
        }
    }
}
