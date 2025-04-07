using Modicom.Repo.Contracts;
using Microsoft.AspNetCore.Mvc;

public class StateViewComponent:ViewComponent
{
     private readonly ISiteContentRepository _siteContentRepository1;

    public StateViewComponent(ISiteContentRepository siteContentRepository)
    {
        
        _siteContentRepository1 = siteContentRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync()
     {
       var item =await  _siteContentRepository1.GetByIdAsync(1);
        return View(item);
     }
}