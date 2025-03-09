// Models/MenuItem.cs
public class MenuItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string IconClass { get; set; }
    public string Link { get; set; }
    public int? ParentId { get; set; }
    public MenuItem Parent { get; set; }
    public List<MenuItem> Children { get; set; } = new();
    public bool IsCollapsible => Children.Any();
}