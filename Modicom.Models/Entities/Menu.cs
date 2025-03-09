using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Modicom.Models.Entities;

[Table("Menu", Schema = "dbo")]
public class Menu
{
    public int Id { get; set; }
    [Display(Name = "نام")]
    public string? Name { get; set; }
    [Display(Name = "فعال")]
    public bool Active { get; set; }
    [Display(Name = "تاریخ ایجاد")]
    public DateTime CreateDate { get; set; }
    [Display(Name = "تاریخ تغییر")]
    public DateTime ModifyDate { get; set; }
    [Display(Name = "اولویت")]
    public int Priority { get; set; }
    [Display(Name = "آدرس اینترنتی")]
     public string? RoutePath { get; set; }// Path to the page or external URL
    [Display(Name = "در پنجره جدید باز شود")]
    public bool OpenInNewWindow { get; set; } // New property to indicate if it should open in a new window

    // Self-referencing relationship for submenus
    [Display(Name = "مربوط به منو اصلی")]
    public int? ParentId { get; set; }
    [Display(Name = "مربوط به منو اصلی")]
    public Menu? Parent { get; set; }
    public ICollection<Menu> SubMenus { get; set; } = new List<Menu>();
}