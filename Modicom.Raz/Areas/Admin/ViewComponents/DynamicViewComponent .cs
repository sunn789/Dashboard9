using Microsoft.AspNetCore.Mvc;
using Modicom.Models.Entities;
using Modicom.Repo.Contracts;

namespace Modicom.Raz.Areas.Admin.ViewComponents;

public class DynamicViewComponent : ViewComponent
{
    private readonly IServiceProvider _serviceProvider;

    public DynamicViewComponent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync(string componentName)
    {
        // First check if we need an entity for this component
        var repositoryType = GetRepositoryType(componentName);

        // If the component does not require a repository (e.g., static content like header, sidebar)
        if (repositoryType == null)
        {
            // Return a view without data
            return View($"~/Areas/Admin/Pages/Shared/Components/{componentName}/Default.cshtml");
        }
        var repoType = componentName.ToLower() switch
        {
            "visitoroverview" => typeof(IVisitorRepository),
            "devicedistribution" => typeof(IVisitorRepository),
            "browserstats" => typeof(IVisitorRepository),
            "countrymap" => typeof(IVisitorRepository),
            "visitortrend" => typeof(IVisitorRepository),
            _ => null
        };

        // First resolve the repository
        var repository = _serviceProvider.GetService(repositoryType);

        if (repoType != null)
        {
            var repo1 = _serviceProvider.GetService(repoType) as IVisitorRepository;


            return View($"~/Areas/Admin/Pages/Shared/Components/{componentName}/Default.cshtml", repoType);

        }
            // Use the generic repository to fetch data
            if (repository is IGenericRepository<object> repo)
            {
                // Use reflection to call GetAllAsync()
                var method = repositoryType.GetMethod("GetAllAsync");
                if (method == null) return View("Error");

                var task = (Task)method.Invoke(repository, null);
                await task.ConfigureAwait(false);

                // Get the result (e.g., List<SiteContent>)
                var resultProperty = task.GetType().GetProperty("Result");
                var data = resultProperty?.GetValue(task);

                // Return the view with the fetched data
                return View($"~/Areas/Admin/Pages/Shared/Components/{componentName}/Default.cshtml", data);
            }

            return View("Error");
        }

    private Type GetRepositoryType(string componentName)
    {
        switch (componentName.ToLower())
        {
            case "sitecontent":
                return typeof(IGenericRepository<SiteContent>);
            case "contactus":
                return typeof(IGenericRepository<ContactUs>);
            case "visitor":
                return typeof(IVisitorRepository);
            case "header":
                return null;  // No repository needed for this component
            case "sidebar":
                return null;  // No repository needed for this component
            case "heademessage":
                return null;  // No repository needed for this component
            default:
                return null;  // Default to no repository needed
        }
    }
    private async Task<object> GetVisitorReportData(string componentName, IVisitorRepository repo)
    {
        return componentName.ToLower() switch
        {
            "visitortrend" => await repo.GetHourlyVisitorsAsync(),
            "devicedistribution" => await repo.GetDeviceDistributionAsync(),
            "browserstats" => await repo.GetBrowserDistributionAsync(),
            "countrymap" => await repo.GetCountryDistributionAsync(),
            "analyticsoverview" => await repo.GetVisitorAnalyticsAsync(),
            _ => null
        };
    }
}
