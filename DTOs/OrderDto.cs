namespace MiniMES.DTOs;

public class OrderDto
{
    public int OrderId { get; set; }
    public string Code { get; set; } ="";
    public string MachineName { get; set; }="";
    public string ProductName { get; set; }="";
    public int Quantity { get; set; }
}

public class CreateOrderDto
{
    public string Code { get; set; } ="";
    public int MachineId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
