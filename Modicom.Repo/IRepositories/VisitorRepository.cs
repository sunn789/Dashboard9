using Microsoft.EntityFrameworkCore;
using Modicom.Models;
using Modicom.Models.Entities;
using Modicom.Repo.Contracts;

public class VisitorRepository : GenericRepository<Visitor>, IVisitorRepository
{
    public VisitorRepository(ApplicationDbContext context) : base(context) { }

    public async Task<VisitorAnalytics> GetVisitorAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = BuildDateRangeQuery(startDate, endDate);
        
        return new VisitorAnalytics
        {
            TotalVisitors = await query.CountAsync(),
            UniqueVisitors = await query.Select(v => v.IpAddress).Distinct().CountAsync(),
            AverageTimeSpent = await query.AverageAsync(v => v.TimeSpent.TotalSeconds),
            MobileVisitors = await query.CountAsync(v => v.IsMobile),
            DesktopVisitors = await query.CountAsync(v => !v.IsMobile),
            BotVisits = await query.CountAsync(v => v.IsBot)
        };
    }

    public async Task<List<Visitor>> GetRecentVisitorsAsync(int count = 10)
    {
        return await GetQueryable()
            .OrderByDescending(v => v.VisitTime)
            .Take(count)
            .ToListAsync();
    }

    public async Task<Dictionary<string, int>> GetDeviceDistributionAsync()
    {
        return await GetQueryable()
            .GroupBy(v => v.DeviceType)
            .Select(g => new { Device = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Device ?? "Unknown", x => x.Count);
    }

    public async Task<Dictionary<string, int>> GetBrowserDistributionAsync()
    {
        return await GetQueryable()
            .GroupBy(v => v.Browser)
            .Select(g => new { Browser = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Browser ?? "Unknown", x => x.Count);
    }

    public async Task<Dictionary<string, int>> GetCountryDistributionAsync()
    {
        return await GetQueryable()
            .GroupBy(v => v.Country)
            .Select(g => new { Country = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Country ?? "Unknown", x => x.Count);
    }

    public async Task<Dictionary<DateTime, int>> GetHourlyVisitorsAsync(int days = 7)
    {
        var endDate = DateTime.UtcNow;
        var startDate = endDate.AddDays(-days);
        
        var rawData = await GetQueryable()
            .Where(v => v.VisitTime >= startDate && v.VisitTime <= endDate)
            .GroupBy(v => new { 
                v.VisitTime.Year, 
                v.VisitTime.Month, 
                v.VisitTime.Day, 
                v.VisitTime.Hour 
            })
            .Select(g => new {
                Hour = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, 0, 0),
                Count = g.Count()
            })
            .ToDictionaryAsync(x => x.Hour, x => x.Count);

        // Fill in missing hours with 0 counts
        var result = new Dictionary<DateTime, int>();
        for (var dt = startDate; dt <= endDate; dt = dt.AddHours(1))
        {
            var hour = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
            result[hour] = rawData.TryGetValue(hour, out var count) ? count : 0;
        }

        return result;
    }

    private IQueryable<Visitor> BuildDateRangeQuery(DateTime? startDate, DateTime? endDate)
    {
        var query = GetQueryable();
        
        if (startDate.HasValue)
            query = query.Where(v => v.VisitTime >= startDate.Value);
        
        if (endDate.HasValue)
            query = query.Where(v => v.VisitTime <= endDate.Value);

        return query;
    }
}