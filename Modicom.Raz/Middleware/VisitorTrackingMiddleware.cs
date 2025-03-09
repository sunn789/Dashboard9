// Modicom.Raz/Middleware/VisitorTrackingMiddleware.cs
public class VisitorTrackingMiddleware
{
    private readonly RequestDelegate _next;

    public VisitorTrackingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IVisitorService visitorService)
    {
        if (!context.Request.Path.StartsWithSegments("/api") &&
            !context.Request.Path.StartsWithSegments("/admin"))
        {
            await visitorService.TrackVisitor(context);
        }
        
        if (context.Session.GetString("LastRequest") != null)
        {
            var lastRequest = DateTime.Parse(context.Session.GetString("LastRequest"));
            if ((DateTime.UtcNow - lastRequest).TotalSeconds < 5) return;
        }
        context.Session.SetString("LastRequest", DateTime.UtcNow.ToString());

        await _next(context);
    }
}