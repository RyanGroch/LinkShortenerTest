using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinkShortener.Data;
using LinkShortener.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LinkShortener.BaseClasses;
using LinkShortener.Utils;
using System.Text.Json;

namespace LinkShortener.Pages
{
    public class DeleteModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) 
        : DI_BasePageModel(context, authorizationService, userManager)
    {
        [BindProperty]
        public ShortLink ShortLink { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            var shortLink = await Context.ShortLink.FirstOrDefaultAsync(m => m.ShortLinkId == id);

            if (shortLink == null)
            {
                return NotFound();
            }

            var userId = UserManager.GetUserId(User);
            var savedLinks = JsonUtils.Deserialize<List<Guid>>(Request.Cookies["SavedLinks"]) ?? [];

            if (!LinkUtils.UserCanAccessLink(userId, shortLink, savedLinks))
            {
                return NotFound();
            }

            ShortLink = shortLink;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            var shortLink = await Context.ShortLink.FirstOrDefaultAsync(m => m.ShortLinkId == id);

            if (shortLink == null)
            {
                return NotFound();
            }

            var userId = UserManager.GetUserId(User);
            var savedLinks = JsonUtils.Deserialize<List<Guid>>(Request.Cookies["SavedLinks"]) ?? [];

            if (!LinkUtils.UserCanAccessLink(userId, shortLink, savedLinks))
            {
                return NotFound();
            }

            ShortLink = shortLink;
            Context.ShortLink.Remove(ShortLink);
            await Context.SaveChangesAsync();

            if (savedLinks.Remove(shortLink.ShortLinkId))
            {
                Response.Cookies.Append("SavedLinks", JsonSerializer.Serialize(savedLinks));
            }

            return RedirectToPage("./Index");
        }
    }
}
