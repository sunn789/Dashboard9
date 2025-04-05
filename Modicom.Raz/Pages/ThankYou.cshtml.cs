using Ins.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ins.Raz.Pages
{
    public class ThankYouModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public ContactUsModel? ContactForm { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool ShowAnimation { get; set; } = true;
        
         public void OnGet()
        {
        }
    }
}
