using System.ComponentModel.DataAnnotations;
namespace Modicom.Models.Entities;

public class ContactUs
{
    public int Id { get; set; }
    [Required]
    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [Required]
    [Display(Name = "Phone")]
    [RegularExpression(@"^\(?([2-9][0-9]{2})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
        ErrorMessage = "Invalid US phone number format.")]
    public string? Phone { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Display(Name = "City")]
    public string? City { get; set; }

    [Display(Name = "State")]
    public string? State { get; set; }

    [Display(Name = "Zip / Postal Code")]
    public string? ZipCode { get; set; }

    [Display(Name = "Preferred Method of Contact")]
    public PreferredContactMethod? PreferredContactMethod { get; set; } // Nullable if not required
    public string?  Subject { get; set; }

    [Display(Name = "How can we help you?")]
    public string? Message { get; set; }

    [Required]
    [Display(Name = "Consent to be contacted")]
    public bool Consent { get; set; }

    [Display(Name = "User IP Address")]
    public string? UserIp { get; set; }

    [Display(Name = "Insert Date")]
    public DateTime InsertDate { get; set; } = DateTime.UtcNow;

    [Display(Name = "Read Status")]
    public bool ReadIt { get; set; } = false;

}
public enum PreferredContactMethod
{
    [Display(Name = "Email")]
    Email,

    [Display(Name = "Phone")]
    Phone,

    [Display(Name = "Text Message")]
    SMS
}