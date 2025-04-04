// GeoService.cs
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Modicom.Services.Services;

public class GeoService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<GeoService> _logger;

    public GeoService(HttpClient httpClient, IMemoryCache cache, ILogger<GeoService> logger)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
    }

    public async Task<string> GetCountryAsync(string ip)
    {
        if (string.IsNullOrEmpty(ip) || IsPrivateIp(ip))
            return "Local";

        var cacheKey = $"geo_{ip}";
        if (_cache.TryGetValue(cacheKey, out string country))
            return country;

        try
        {
            var apiUrl = string.Format(_httpClient.BaseAddress.OriginalString, ip);
            var response = await _httpClient.GetFromJsonAsync<GeoResponse>(apiUrl);
            
            _cache.Set(cacheKey, response.Country, TimeSpan.FromMinutes(1440));
            return response.Country;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطا در دریافت کشور برای IP: {IP}", ip);
            return "Unknown";
        }
    }

    private bool IsPrivateIp(string ip) => 
        IPAddress.TryParse(ip, out var address) && 
        (address.IsPrivate() || IPAddress.IsLoopback(address));
}

public static class IpAddressExtensions
{
    public static bool IsPrivate(this IPAddress ip)
    {
        if (ip.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            return false;

        byte[] bytes = ip.GetAddressBytes();
        return bytes[0] switch
        {
            10 => true,
            172 => bytes[1] >= 16 && bytes[1] <= 31,
            192 when bytes[1] == 168 => true,
            127 => true,
            _ => false
        };
    }
}

public class GeoResponse
{
    [JsonPropertyName("country")]
    public string Country { get; set; }
}