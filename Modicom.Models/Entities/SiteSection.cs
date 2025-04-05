namespace Modicom.Models.Entities;
public class SiteSection
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool Active { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdate { get; set; }
    public int Prority { get; set; }
    public string? Description { get; set; }

}
