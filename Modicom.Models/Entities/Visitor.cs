using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
namespace Modicom.Models.Entities;

public class Visitor
{

    [Key]
    public int Id { get; set; }

    // ========== شناسایی کاربر ==========
    [Required]
    [StringLength(45)] // IPv4: 15, IPv6: 45
    public string? IpAddress { get; set; }

    [StringLength(64)]
    public string? HashedIp { get; set; } // برای حریم خصوصی (GDPR)
    [Required]
    [StringLength(64)]
    public string? VisitorFingerprint { get; set; } // ترکیب IP + UserAgent + DeviceType + SessionId (هششده)
    [StringLength(2)]
    public string? CountryCode { get; set; } // کد 2 حرفی کشور (مثل IR)

    public string? Country { get; set; }


    // ========== اطلاعات دستگاه ==========
    [Required]
    [StringLength(500)]
    public string? UserAgent { get; set; }

    [StringLength(50)]
    public string? BrowserFamily { get; set; }

    [StringLength(20)]
    public string? BrowserVersion { get; set; }

    [StringLength(50)]
    public string? OperatingSystem { get; set; }

    [StringLength(20)]
    public string? DeviceType { get; set; } // Desktop, Mobile, Tablet, Bot

    public bool IsBot { get; set; }
    [Required]
    public bool IsMobile { get; set; } // افزودن این خط

    // ========== اطلاعات ناوبری ==========
    [StringLength(2000)]
    public string? Referrer { get; set; }

    [Required]
    [StringLength(2000)]
    public string? PageUrl { get; set; }

    [Required]
    [StringLength(10)]
    public string? HttpMethod { get; set; }

    // ========== زمانبندی ==========
    [Required]
    public DateTime VisitTime { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime LastActivityTime { get; set; } = DateTime.UtcNow;

    [NotMapped] // محاسبه در زمان اجرا
    public TimeSpan TimeSpent => LastActivityTime - VisitTime;

    // ========== مدیریت نشست ==========
    [Required]
    [StringLength(64)]
    public string? SessionId { get; set; }

    [Required]
    public bool IsNewSession { get; set; }

    // ========== توکن همزمانی برای Race Conditions ==========
    [Timestamp]
    public byte[]? ConcurrencyToken { get; set; }
}

