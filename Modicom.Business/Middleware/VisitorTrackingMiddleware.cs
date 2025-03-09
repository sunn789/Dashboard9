namespace Modicom.Business.Middleware;

public class VisitorTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<VisitorTrackingMiddleware> _logger;

    public VisitorTrackingMiddleware(RequestDelegate next, ILogger<VisitorTrackingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, AnalyticsDbContext dbContext)
    {
        // Skip static files and API requests
        if (context.Request.Path.StartsWithSegments("/api") ||
            Path.HasExtension(context.Request.Path.Value))
        {
            await _next(context);
            return;
        }

        var visitor = new Visitor
        {
            IpAddress = context.Connection.RemoteIpAddress?.ToString(),
            UserAgent = context.Request.Headers["User-Agent"],
            Referrer = context.Request.Headers["Referer"].ToString(),
            EntryPage = context.Request.Path,
            EntryTime = DateTime.UtcNow,
            OperatingSystem = GetOperatingSystem(context.Request.Headers["User-Agent"]),
            DeviceType = GetDeviceType(context.Request.Headers["User-Agent"]),
            Browser = GetBrowser(context.Request.Headers["User-Agent"])
        };

        try
        {
            dbContext.Visitors.Add(visitor);
            await dbContext.SaveChangesAsync();
            
            // Update daily counter
            var today = DateTime.UtcNow.Date;
            var dailyCount = await dbContext.DailyVisitorCounts
                .FirstOrDefaultAsync(d => d.Date == today);

            if (dailyCount == null)
            {
                dailyCount = new DailyVisitorCount { Date = today, Count = 1 };
                dbContext.DailyVisitorCounts.Add(dailyCount);
            }
            else
            {
                dailyCount.Count++;
            }

            await dbContext.SaveChangesAsync();
            
            context.Items["VisitorId"] = visitor.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking visitor");
        }

        await _next(context);
    }

    private string GetOperatingSystem(string userAgent)
    {
        var uaParser = Parser.GetDefault();
        var clientInfo = uaParser.Parse(userAgent);
        return clientInfo.OS.ToString();
    }

    private string GetDeviceType(string userAgent)
    {
        // Implement device detection logic or use a library
        if (userAgent.Contains("Mobi", StringComparison.OrdinalIgnoreCase))
            return "Mobile";
        if (userAgent.Contains("Tablet", StringComparison.OrdinalIgnoreCase))
            return "Tablet";
        return "Desktop";
    }

    private string GetBrowser(string userAgent)
    {
        var uaParser = Parser.GetDefault();
        var clientInfo = uaParser.Parse(userAgent);
        return clientInfo.UA.ToString();
    }
}