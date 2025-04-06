namespace Modicom.Models.Entities;


public class Testimonials
{
    public int Id { get; set; }
    public string? SenderName { get; set; }
    public string? Text { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime ModifyDate { get; set; }
    public int Prority { get; set; }
    public bool Active { get; set; }
}