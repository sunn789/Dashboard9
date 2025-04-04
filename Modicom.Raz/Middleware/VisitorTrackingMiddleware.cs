using Microsoft.Extensions.Options;
using Modicom.Services.Configuration;

public class VisitorTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<VisitorTrackingMiddleware> _logger;
    private readonly TrackingExclusions _exclusions;

    public VisitorTrackingMiddleware(
        RequestDelegate next,
        ILogger<VisitorTrackingMiddleware> logger,
        IOptions<TrackingExclusions> exclusions)
    {
        _next = next;
        _logger = logger;
        _exclusions = exclusions.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if (ShouldTrack(context))
            {
                // دریافت سرویس از طریق RequestServices
                var visitService = context.RequestServices.GetRequiredService<VisitService>();
                
                context.Response.OnStarting(async () => 
                {
                     visitService.ProcessVisit(context);
                });
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطا در ردیابی بازدید");
            throw;
        }
    }

    private bool ShouldTrack(HttpContext context)
    {
        var path = context.Request.Path.Value ?? "";
        return !_exclusions.Paths!.Contains(path) &&
               !_exclusions.Prefixes!.Any(p => path.StartsWith(p));
    }
}