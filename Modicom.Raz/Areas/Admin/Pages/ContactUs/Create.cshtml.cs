using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modicom.Models;
using Modicom.Models.Entities;

namespace Modicom.Raz.Areas_Admin_Pages_ContactUs
{
    public class CreateModel : PageModel
    {
        private readonly Modicom.Models.ApplicationDbContext _context;

        public CreateModel(Modicom.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ContactUs ContactUs { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ContactUs.Add(ContactUs);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
