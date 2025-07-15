namespace MiniMES.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<int> OrderIds { get; set; } = new();
}

public class CreateProductDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}