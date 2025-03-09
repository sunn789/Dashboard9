using System.ComponentModel.DataAnnotations;
namespace Modicom.Models.Entities;

public class Visitor
{

    [Key]
    public int Id { get; set; }

    // IP Information
    public string? IpAddress { get; set; }  // Standardize casing (not both IPAddress and IpAddress)
    public string? Country { get; set; }

    // Device/Browser Information
    public string? Browser { get; set; }
    public string? BrowserVersion { get; set; }  // Added for more detail
    public string? OperatingSystem { get; set; }
    public string? DeviceType { get; set; }
    public bool IsMobile { get; set; }
    public bool IsBot { get; set; }
    public string? UserAgent { get; set; }

    // Navigation Information
    public string? Referrer { get; set; }  // Standardize spelling (not both Referer/Referrer)
    public string? PageUrl { get; set; }
    public string? HttpMethod { get; set; }

    // Timing Information
    public DateTime VisitTime { get; set; }
    public DateTime LastActivityTime { get; set; }
    public TimeSpan TimeSpent { get; set; }  // Better than seconds as TimeSpan

    // Session Information
    public bool IsNewSession { get; set; }
    public string? SessionId { get; set; }  // Added for session tracking
}

