namespace MiniMES.DTOs;

public class MachineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
}

public class CreateMachineDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}