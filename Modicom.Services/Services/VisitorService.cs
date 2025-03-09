// Modicom.Services/Services/VisitorService.cs
using Microsoft.AspNetCore.Http;
using Modicom.Models.Entities;

public interface IVisitorService
{
    Task TrackVisitor(HttpContext context);
    int GetTotalVisitors();
}

public class VisitorService : IVisitorService
{
    private readonly IGenericRepository<Visitor> _repo;
    
    public VisitorService(IGenericRepository<Visitor> repo)
    {
        _repo = repo;
    }

    public async Task TrackVisitor(HttpContext context)
    {
        var visitor = new Visitor
        {
            IpAddress = GetIpAddress(context),
            UserAgent = context.Request.Headers["User-Agent"],
            VisitTime = DateTime.UtcNow,
            LastActivityTime = DateTime.UtcNow,
            Referrer = context.Request.Headers["Referer"],
            PageUrl = context.Request.Path,
            IsNewSession = context.Session.GetString("SessionID") == null
        };

        ParseUserAgent(visitor);
        CheckForBot(visitor);
        
        await _repo.AddAsync(visitor);
        await _repo.SaveAsync();

        context.Session.SetString("SessionID", Guid.NewGuid().ToString());
    }

    private string GetIpAddress(HttpContext context)
    {
        return context.Connection.RemoteIpAddress?.ToString() ?? 
               context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    }

    private void ParseUserAgent(Visitor visitor)
    {
        // Use a library like UAParser (install via NuGet)
        var uaParser = Parser.GetDefault();
        var clientInfo = uaParser.Parse(visitor.UserAgent);
        
        visitor.OperatingSystem = clientInfo.OS.ToString();
        visitor.Browser = clientInfo.UA.ToString();
        visitor.DeviceType = clientInfo.Device.ToString();
        visitor.IsMobile = clientInfo.Device.IsMobile;
    }

    private void CheckForBot(Visitor visitor)
    {
        visitor.IsBot = visitor.UserAgent.Contains("bot", StringComparison.OrdinalIgnoreCase) || 
                       visitor.UserAgent.Contains("spider", StringComparison.OrdinalIgnoreCase);
    }

    public int GetTotalVisitors() => _repo.GetCount();
    private string GetCountry(string ipAddress)
{
    try
    {
        using var reader = new DatabaseReader("GeoLite2-Country.mmdb");
        var country = reader.Country(ipAddress);
        return country.Country.Name;
    }
    catch
    {
        return "Unknown";
    }
}
}