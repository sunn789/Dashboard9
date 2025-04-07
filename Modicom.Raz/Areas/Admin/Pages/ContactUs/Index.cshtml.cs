using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Modicom.Models;
using Modicom.Models.Entities;

namespace Modicom.Raz.Areas_Admin_Pages_ContactUs
{
    public class IndexModel : PageModel
    {
        private readonly Modicom.Models.ApplicationDbContext _context;

        public IndexModel(Modicom.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ContactUs> ContactUs { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ContactUs = await _context.ContactUs.ToListAsync();
        }
    }
}
