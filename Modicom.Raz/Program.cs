using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Modicom.Models;
using Modicom.Raz.Areas.Admin.ViewComponents;
using Modicom.Repo.Contracts;
using Modicom.Services.Configuration;
using Modicom.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. پیکربندی دیتابیس
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") 
    ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(connectionString));

// 2. تنظیمات Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = true;
}).AddEntityFrameworkStores<ApplicationDbContext>();

// 3. ثبت سرویس‌های پیکربندی
builder.Services
    .Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"))
    .Configure<GeoIPSettings>(builder.Configuration.GetSection("GeoIP"))
    .Configure<TrackingExclusions>(builder.Configuration.GetSection("TrackingExclusions"));

// 4. سرویس‌های برنامه
builder.Services.AddHttpClient<GeoService>((sp, client) => {
    var geoConfig = sp.GetRequiredService<IOptions<GeoIPSettings>>().Value;
    client.BaseAddress = new Uri(geoConfig.ServiceUrl!);
});

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
})
    .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
    .AddScoped<IVisitorRepository, VisitorRepository>()
    .AddScoped<ISiteContentRepository, SiteContentRepository>()
    .AddScoped<IContactUsRepository, ContactUsRepository>()
    .AddScoped<VisitService>()
    .AddScoped<DynamicViewComponent>()
    .AddRazorPages();

// 5. پیکربندی Redis
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration.GetConnectionString("Redis"); 
    options.InstanceName = "VisitorTracker_";
});

var app = builder.Build();

// 6. میدلورها و خط‌مشی امنیتی
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapStaticAssets();
app.UseSession();
app.UseMiddleware<VisitorTrackingMiddleware>();
app.MapRazorPages().WithStaticAssets();

app.Run();