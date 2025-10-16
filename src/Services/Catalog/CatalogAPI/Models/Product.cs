namespace CatalogAPI.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<string> Category { get; set; } = [];
    public string ImageFile { get; set; } = string.Empty;
}
