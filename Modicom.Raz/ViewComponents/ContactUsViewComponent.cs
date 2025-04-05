using Ins.Repo.Contrcts;
using Microsoft.AspNetCore.Mvc;

public class ContactUsViewComponent:ViewComponent
{
     private readonly ISiteContentRepository _siteContentRepository1;

    public ContactUsViewComponent(ISiteContentRepository siteContentRepository)
    {
        
        _siteContentRepository1 = siteContentRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync()
     {
       var item =await  _siteContentRepository1.GetStaticById(1);
        return View(item);
     }
}