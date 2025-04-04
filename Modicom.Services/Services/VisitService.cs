using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Modicom.Models;
using Modicom.Models.Entities;
using Modicom.Services.Configuration;
using UAParser;


public class VisitService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDistributedCache _cache;
     private readonly TrackingExclusions _exclusions;
    private readonly object _lock = new();

public VisitService(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor,
    IDistributedCache cache,
    IOptions<TrackingExclusions> exclusions) // تغییر به IOptions<T>
{
    _context = context;
    _httpContextAccessor = httpContextAccessor;
    _cache = cache;
    _exclusions = exclusions.Value; // دریافت مقدار واقعی
}

    public void ProcessVisit(HttpContext context)
    {
         var path = context.Request.Path.Value;
        if (_exclusions.Paths!.Contains(path!) || 
            _exclusions.Prefixes!.Any(p => path!.StartsWith(p)))
        {
            return;
        }
        lock (_lock) // مدیریت Race Condition
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var ip = httpContext!.Connection.RemoteIpAddress?.ToString();

            // بررسی کش برای جلوگیری از پردازش تکراری
            var cacheKey = $"VISITOR_{ip}_{DateTime.UtcNow:yyyyMMdd}";
            if (_cache.GetString(cacheKey) != null) return;

            var visitor = CreateVisitor(httpContext);
            ValidateVisitor(visitor);

            _context.Visitors.Add(visitor);
            _context.SaveChanges();

            // ذخیره در کش به مدت 24 ساعت
            _cache.SetString(cacheKey, "1", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            });
        }
    }

    private Visitor CreateVisitor(HttpContext context)
    {
        // تولید شناسه سشن با قوانین GDPR
        var sessionId = Guid.NewGuid().ToString("N");
        var userAgent = context.Request.Headers.UserAgent.ToString();

        return new Visitor
        {
            IpAddress = context.Connection.RemoteIpAddress?.ToString(),
            HashedIp = HashIp(context.Connection.RemoteIpAddress?.ToString()!),
            VisitorFingerprint = GenerateFingerprint(
                context.Connection.RemoteIpAddress?.ToString()!,
                userAgent,
                sessionId
            ),
            UserAgent = userAgent,
            BrowserFamily = GetBrowserFamily(userAgent),
            BrowserVersion = GetBrowserVersion(userAgent),
            OperatingSystem = GetOperatingSystem(userAgent),
            DeviceType = DetectDeviceType(userAgent),
            IsBot = IsBotRequest(userAgent),
            Referrer = context.Request.Headers.Referer,
            PageUrl = context.Request.Path,
            HttpMethod = context.Request.Method,
            SessionId = sessionId,
            IsNewSession = true,
            VisitTime = DateTime.UtcNow,
            LastActivityTime = DateTime.UtcNow
        };
    }

    private string HashIp(string ip)
    {
        using var sha256 = SHA256.Create();
    var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(ip));
    return BitConverter.ToString(hashBytes).Replace("-", ""); // Hex format (64 chars)
    }

    private string GenerateFingerprint(string ip, string userAgent, string sessionId)
    {
        var rawData = $"{ip}-{userAgent}-{sessionId}";
        using var sha256 = SHA256.Create();
        return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData))) .Replace("-", "")
        .Substring(0, 64);;
    }

    private bool IsBotRequest(string userAgent)
    {
        var knownBots = new[] { "Googlebot", "Bingbot", "YandexBot" };
        return knownBots.Any(b => userAgent.Contains(b));
    }

    private void ValidateVisitor(Visitor visitor)
    {
        if (string.IsNullOrEmpty(visitor.IpAddress))
            throw new ArgumentException("IP Address is required");

        if (visitor.IpAddress.Length > 45)
            throw new ArgumentException("Invalid IP Address length");
    }
    private string GetBrowserFamily(string userAgent)
    {
        var parser = Parser.GetDefault();
        var clientInfo = parser.Parse(userAgent);
        return clientInfo.Browser.Family;
    }
    private string GetBrowserVersion(string userAgent)
    {
        var parser = Parser.GetDefault();
        var clientInfo = parser.Parse(userAgent);
        return $"{clientInfo.Browser.Major}.{clientInfo.Browser.Minor}.{clientInfo.Browser.Patch}";
    }
    private string GetOperatingSystem(string userAgent)
    {
        var parser = Parser.GetDefault();
        var clientInfo = parser.Parse(userAgent);
        return clientInfo.OS.Family;
    }
    private string DetectDeviceType(string userAgent)
    {
        var parser = Parser.GetDefault();
        var clientInfo = parser.Parse(userAgent);

        return clientInfo.Device.Family switch
        {
            "iPhone" => "Mobile",
            "iPad" => "Tablet",
            "Android" => "Mobile",
            _ => "Desktop"
        };
    }
}