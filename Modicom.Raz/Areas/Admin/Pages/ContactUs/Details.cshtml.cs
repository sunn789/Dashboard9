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
    public class DetailsModel : PageModel
    {
        private readonly Modicom.Models.ApplicationDbContext _context;

        public DetailsModel(Modicom.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public ContactUs ContactUs { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactus = await _context.ContactUs.FirstOrDefaultAsync(m => m.Id == id);

            if (contactus is not null)
            {
                ContactUs = contactus;

                return Page();
            }

            return NotFound();
        }
    }
}
