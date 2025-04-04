using Modicom.Models.Entities;

namespace Modicom.Repo.Contracts;

// IVisitorRepository.cs
public interface IVisitorRepository : IGenericRepository<Visitor>
{
    Task<VisitorAnalytics> GetVisitorAnalyticsAsync(DateTime? start = null, DateTime? end = null);
    Task<List<Visitor>> GetRecentVisitorsAsync(int count = 10);
    Task<Dictionary<string, int>> GetDeviceDistributionAsync();
    Task<Dictionary<string, int>> GetBrowserDistributionAsync();
    Task<Dictionary<string, int>> GetCountryDistributionAsync();
    Task<Dictionary<DateTime, int>> GetHourlyVisitorsAsync(int days = 7);
}

public class VisitorAnalytics
{
    public int TotalVisitors { get; set; }
    public int UniqueVisitors { get; set; }
    public double AvgTimeSpent { get; set; }
    public int MobileVisitors { get; set; }
    public int DesktopVisitors { get; set; }
    public int BotVisits { get; set; }
    public Dictionary<string, int> TopPages { get; set; }
}