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
    public class IndexModel : PageModel
    {
        private readonly LinkShortener.Data.ApplicationDbContext _context;

        public IndexModel(LinkShortener.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ShortLink> ShortLink { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ShortLink = await _context.ShortLink.ToListAsync();
        }
    }
}
