using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LinkShortener.Data;
using LinkShortener.Models;

namespace LinkShortener.Pages.ShortLinks
{
    public class DeleteModel : PageModel
    {
        private readonly LinkShortener.Data.ApplicationDbContext _context;

        public DeleteModel(LinkShortener.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ShortLink ShortLink { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shortlink = await _context.ShortLink.FirstOrDefaultAsync(m => m.ShortLinkId == id);

            if (shortlink is not null)
            {
                ShortLink = shortlink;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shortlink = await _context.ShortLink.FindAsync(id);
            if (shortlink != null)
            {
                ShortLink = shortlink;
                _context.ShortLink.Remove(ShortLink);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
