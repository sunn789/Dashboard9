using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modicom.Models.Entities;
using Modicom.Repo.Contracts;

namespace Modicom.Raz.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
        private readonly ISiteContentRepository _repository;
        private readonly IContactUsRepository _contactUsRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IndexModel(ILogger<IndexModel> logger, ISiteContentRepository repository, IHttpContextAccessor httpContextAccessor, IContactUsRepository contactUsRepository)
    {
        _logger = logger;
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
        _contactUsRepository = contactUsRepository;
    }
    public SiteSection[]? SiteSections { get; set; }
     [TempData]
    public string? ToastMessage { get; set; }
    public ContactUs  ContactUs { get; set; }
    public void OnGet()
    {

    }
    public async Task<IActionResult> OnPostContact()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Capture IP address
            ContactUs.Address = "";
            ContactUs.City = "";
            ContactUs.Consent = false;
            ContactUs.LastName = "";
            ContactUs.Phone = "";
            ContactUs.PreferredContactMethod = PreferredContactMethod.Email;
            ContactUs.State = "";
            ContactUs.ZipCode = "";

            ContactUs.UserIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            ContactUs.InsertDate = DateTime.UtcNow;

            await _contactUsRepository.AddAsync(ContactUs);
            
            TempData["ToastMessage"] = "Your message has been received! We'll contact you soon.";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error submitting form. Please try again." + ex.ToString());
            return Page();
        }
    }
}
