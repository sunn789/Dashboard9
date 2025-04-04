
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Modicom.Models.Entities;

public class VisitorTrackerService
{
    private readonly Modicom.Models.ApplicationDbContext _context;
    private readonly IDistributedCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VisitorTrackerService(
       Modicom.Models.ApplicationDbContext context,
        IDistributedCache cache,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task TrackVisit()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var ip = httpContext.Connection.RemoteIpAddress?.ToString();

        // کش کردن IP برای جلوگیری از درخواستهای مکرر
        var cacheKey = $"Visitor_{ip}";
        if (await _cache.GetStringAsync(cacheKey) != null)
            return;

        // پردازش و ذخیره اطلاعات
        var visitor = new Visitor
        {
            IpAddress = ip,
            UserAgent = httpContext.Request.Headers.UserAgent,
            // ...
        };

        await _context.Visitors.AddAsync(visitor);
        await _context.SaveChangesAsync();

        // کش به مدت 5 دقیقه
        await _cache.SetStringAsync(cacheKey, "processed", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });
    }
}

// using Microsoft.AspNetCore.Http;
// using Modicom.Models.Entities;
// namespace Modicom.Services.Services;
// public interface IVisitorService
// {
//     Task TrackVisitorAsync(HttpContext context);
//     Task<VisitorAnalytics> GetVisitorAnalyticsAsync(DateTime? start = null, DateTime? end = null);
//     Task<Dictionary<string, int>> GetDeviceDistributionAsync();
//     Task<Dictionary<string, int>> GetBrowserDistributionAsync();
//     Task<Dictionary<string, int>> GetCountryDistributionAsync();
//     Task<Dictionary<DateTime, int>> GetHourlyVisitorsAsync(int days = 7);
//     Task<List<Visitor>> GetRecentVisitorsAsync(int count = 10);
// }

// public class VisitorService : IVisitorService
// {
//     private readonly IVisitorRepository _repo;
//     private readonly GeoService _geoService;
//     private readonly IHttpContextAccessor _httpContextAccessor;

//     public VisitorService(
//         IVisitorRepository repo, 
//         GeoService geoService,
//         IHttpContextAccessor httpContextAccessor)
//     {
//         _repo = repo;
//         _geoService = geoService;
//         _httpContextAccessor = httpContextAccessor;
//     }

//     public async Task TrackVisitorAsync(HttpContext context)
//     {
//         var visitor = new Visitor
//         {
//             IpAddress = context.Connection.RemoteIpAddress?.ToString(),
//             UserAgent = context.Request.Headers.UserAgent,
//             VisitTime = DateTime.UtcNow,
//             PageUrl = context.Request.Path,
//             Referrer = context.Request.Headers.Referer,
//             HttpMethod = context.Request.Method,
//             SessionId = context.Session.Id,
//             IsNewSession = !context.Session.Keys.Contains("VisitorTracked")
//         };

//         // Parse user agent
//         var uaParser = Parser.GetDefault().Parse(visitor.UserAgent);
//         visitor.Browser = $"{uaParser.UA.Family} {uaParser.UA.Major}";
//         visitor.OperatingSystem = uaParser.OS.ToString();
//         visitor.IsMobile = uaParser.Device.IsMobile;

//         // Get country
//         visitor.Country = await _geoService.GetCountryAsync(visitor.IpAddress);

//         // Check if should track
//         if (!IsDuplicateRequest(context))
//         {
//             await _repo.AddAsync(visitor);
//             await _repo.SaveAsync();
//             context.Session.SetString("VisitorTracked", "true");
//         }
//     }

//     private bool IsDuplicateRequest(HttpContext context)
//     {
//         return context.Session.Keys.Contains("VisitorTracked");
//     }

//     // Other methods simply call repository
//     public Task<VisitorAnalytics> GetVisitorAnalyticsAsync(DateTime? start = null, DateTime? end = null)
//         => _repo.GetVisitorAnalyticsAsync(start, end);
    
//     public Task<Dictionary<string, int>> GetDeviceDistributionAsync()
//         => _repo.GetDeviceDistributionAsync();
    
//     // ... implement other interface methods similarly ...
// }