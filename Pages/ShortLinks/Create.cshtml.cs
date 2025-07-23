using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LinkShortener.Data;
using LinkShortener.Models;

namespace LinkShortener.Pages.ShortLinks
{
    public class CreateModel : PageModel
    {
        private readonly LinkShortener.Data.ApplicationDbContext _context;

        public CreateModel(LinkShortener.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ShortLink ShortLink { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ShortLink.Add(ShortLink);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
