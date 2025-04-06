
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modicom.Models.Entities;

namespace Ins.Raz.Pages
{
    public class ThankYouModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public ContactUs? ContactForm { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool ShowAnimation { get; set; } = true;
        
         public void OnGet()
        {
        }
    }
}
