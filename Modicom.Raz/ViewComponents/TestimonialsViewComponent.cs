using Ins.Repo.Contrcts;

using Microsoft.AspNetCore.Mvc;

namespace Ins.Raz.ViewComponents;

public class TestimonialsViewComponent:ViewComponent
{
   private readonly ISiteContentRepository _siteContentRepository1;

    public TestimonialsViewComponent(ISiteContentRepository siteContentRepository)
    {
        
        _siteContentRepository1 = siteContentRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync()
     {
        return View();
     }
}
