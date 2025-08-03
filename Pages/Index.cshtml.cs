using System.Text.Json;
using SmallUrl.Data;
using SmallUrl.Models;
using SmallUrl.Utils;
using SmallUrl.BaseClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SmallUrl.Pages;

public class IndexModel(
        ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager) 
    : DI_BasePageModel(context, authorizationService, userManager)
{
    [BindProperty]
    public ShortLinkView ShortLinkView { get; set; } = default!;

    public ShortLink ShortLink { get; set; } = default!;

    public string HostUrl { get; set; } = default!;

    public void OnGet()
    {
        HostUrl = Request.Host.ToString();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        HostUrl = Request.Host.ToString();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        string destination = LinkUtils.CleanDestination(ShortLinkView.Destination);
        string slug = LinkUtils.CleanSlug(ShortLinkView.Slug);

        string? userId = UserManager.GetUserId(User);

        ShortLink = new()
        {
            Destination = destination,
            Slug = slug,
            OwnerId = userId
        };

        Context.ShortLink.Add(ShortLink);
        await Context.SaveChangesAsync();

        if (userId == null)
        {
            var savedLinks = JsonUtils.Deserialize<List<string>>(Request.Cookies["SavedLinks"]) ?? [];
            savedLinks.Add(ShortLink.ShortLinkId.ToString());

            var serializedLinks = JsonSerializer.Serialize(savedLinks);

            // Ensure that updated cookies apply to both this request and all following requests
            HttpContext.Items["SavedLinks"] = serializedLinks;

            Response.Cookies.Append("SavedLinks", serializedLinks, new()
            {
                // Closest we can get to never expiring
                Expires = new DateTimeOffset(2038, 1, 1, 0, 0, 0, TimeSpan.FromHours(0))
            });
        }

        return Page();
    }
}
