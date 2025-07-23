using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LinkShortener.Data;
using LinkShortener.Models;
using LinkShortener.Utils;
using LinkShortener.BaseClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace LinkShortener.Pages
{
    public class EditModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) 
        : DI_BasePageModel(context, authorizationService, userManager)
    {
        [BindProperty]
        public ShortLinkView ShortLinkView { get; set; } = default!;

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

            ShortLinkView = new()
            {
                ShortLinkId = id,
                Destination = shortLink.Destination,
                Slug = shortLink.Slug
            };

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            // Make sure that the ID in the form is 
            // consistent with the ID in the request
            if (id != ShortLinkView.ShortLinkId)
            {
                return NotFound();
            }

            var shortLink = await Context.ShortLink.FirstOrDefaultAsync(l => l.ShortLinkId == id);

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

            if (!ModelState.IsValid)
            {
                return Page();
            }

            string destination = LinkUtils.CleanDestination(ShortLinkView.Destination);
            string slug = LinkUtils.CleanSlug(ShortLinkView.Slug);

            shortLink.Destination = destination;
            shortLink.Slug = slug;
            Context.Attach(shortLink).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Context.ShortLink.Any(e => e.ShortLinkId == shortLink.ShortLinkId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (userId == null && !savedLinks.Contains(shortLink.ShortLinkId))
            {
                savedLinks.Add(shortLink.ShortLinkId);
                Response.Cookies.Append("SavedLinks", JsonSerializer.Serialize(savedLinks));
            }

            return RedirectToPage("./Index");
        }
    }
}
