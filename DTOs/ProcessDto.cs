namespace MiniMES.DTOs;

public class ProcessDto
{
    public int Id { get; set; }
    public int SerialNumber { get; set; }
    public string Status { get; set; } = "";
    public DateTime DateTime { get; set; }
    public OrderDto Order { get; set; } = new OrderDto(); 
    public List<ProcessParameterDto> ProcessParameters = new List<ProcessParameterDto>();
}

public class CreateProcessDto
{
    public int SerialNumber { get; set; }
    public int OrderId { get; set; }
    public string Status { get; set; } = null!;
}