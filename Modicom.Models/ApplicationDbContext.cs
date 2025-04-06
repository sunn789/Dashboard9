
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modicom.Models.Entities;
namespace Modicom.Models;


[Table("AspNetUsers")]
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public int ApplicationUserId { get; set; }
    [PersonalData]
    public string? FirstName { get; set; }
    [PersonalData]
    public string? LastName { get; set; }
    [PersonalData]
    public string? FatherName { get; set; }
    [PersonalData]
    public string? NCode { get; set; }
    [PersonalData]
    public string? Phone { get; set; }
    [PersonalData]
    public string? Address { get; set; }
    [PersonalData]
    public bool Active { get; set; }
    [PersonalData]
    public string? Description { get; set; }
}
public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
        builder.Entity<Visitor>()
        .HasIndex(v => v.VisitTime);
    
    builder.Entity<Visitor>()
        .HasIndex(v => v.IpAddress);

    }
    public DbSet<SiteContent> SiteContents { get; set; }
    public DbSet<ContactUs> ContactUs { get; set; }
    // public DbSet<DailyVisitors> DailyVisitors { get; set; }
    public DbSet<Visitor> Visitors { get; set; }


}

