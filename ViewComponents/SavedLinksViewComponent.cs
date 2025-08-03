using System.Security.Claims;
using SmallUrl.Data;
using SmallUrl.Models;
using SmallUrl.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SmallUrl.ViewComponents
{
    public class SavedLinksViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        protected UserManager<IdentityUser> _userManager { get; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SavedLinksViewComponent(ApplicationDbContext context,
                             UserManager<IdentityUser> userManager,
                             IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            var userId = user is not null ? _userManager.GetUserId(user) : null;

            var serverItems = HttpContext.Items["SavedLinks"];

            string? savedLinkSource =
                serverItems is string
                    ? (string?) serverItems
                    : Request.Cookies["SavedLinks"] is not null
                    ? Request.Cookies["SavedLinks"]
                    : null;

            var cookieLinks = JsonUtils.Deserialize<List<Guid>>(savedLinkSource) ?? [];

            var unownedLinksQuery = from link in _context.ShortLink
                                    where cookieLinks.Contains(link.ShortLinkId)
                                        && link.OwnerId == null
                                    select link;

            var ownedLinksQuery = from link in _context.ShortLink
                                  where userId != null && userId == link.OwnerId
                                  select link;


            var savedLinksData = new SavedLinksData()
            {
                UnownedLinks = await unownedLinksQuery.ToListAsync(),
                OwnedLinks = await ownedLinksQuery.ToListAsync(),
                HostUrl = Request.Host.ToString()
            };

            return View(savedLinksData);
        }
    }
}
