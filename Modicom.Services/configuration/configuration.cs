namespace Modicom.Services.Configuration;


    public class EmailSettings
{
    public string? SenderEmail { get; set; }
    public string? SenderPassword { get; set; }
    public string? SmtpServer { get; set; }
    public int SmtpPort { get; set; }
}

public class GeoIPSettings
{
    public string? ServiceUrl { get; set; }
    public int CacheMinutes { get; set; }
}

public class TrackingExclusions
{
    public List<string>? Paths { get; set; }
    public List<string>? Prefixes { get; set; }
}
