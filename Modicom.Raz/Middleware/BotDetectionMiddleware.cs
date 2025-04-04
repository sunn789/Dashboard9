using System;
using Modicom.Models;
using Modicom.Models.Entities;

public class BotDetectionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<string> _botUserAgents = new() { "Googlebot", "Bingbot", "Slurp" };

    public BotDetectionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext db)
    {
        var userAgent = context.Request.Headers.UserAgent.ToString();
        var isBot = _botUserAgents.Any(ua => userAgent.Contains(ua));

        if (isBot)
        {
            var visitor = new Visitor { /* ... */ IsBot = true };
            await db.Visitors.AddAsync(visitor);
            await db.SaveChangesAsync();
            context.Response.StatusCode = 403;
            return;
        }

        await _next(context);
    }
}