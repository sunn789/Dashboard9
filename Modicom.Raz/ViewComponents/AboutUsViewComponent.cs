using Ins.Repo.Contrcts;

using Microsoft.AspNetCore.Mvc;

namespace Ins.Raz.ViewComponents;

public class AboutUsViewComponent:ViewComponent
{
   private readonly ISiteContentRepository _siteContentRepository1;

    public AboutUsViewComponent(ISiteContentRepository siteContentRepository)
    {
        
        _siteContentRepository1 = siteContentRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync()
     {
       var item =await  _siteContentRepository1.GetStaticById(1);
        return View(item);
     }
}
