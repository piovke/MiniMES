namespace MiniMES.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public string Code { get; set; } ="";
    public MachineDto Machine { get; set; } = new MachineDto();
    public ProductDto Product { get; set; } = new  ProductDto();
    public List<ProcessDto> Processes { get; set; } = new List<ProcessDto>();
    public int Quantity { get; set; }
}

public class CreateOrderDto
{
    public string Code { get; set; } ="";
    public int MachineId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
