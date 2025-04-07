using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modicom.Models.Entities;

namespace Modicom.Raz.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }
    public SiteSection[]? SiteSections { get; set; }
    public void OnGet()
    {

    }
}
