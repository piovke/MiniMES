namespace MiniMES.DTOs;

public class MachineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<int> OrderIds { get; set; } = new();
    public List<string> Orders { get; set; } = new();
}

public class CreateMachineDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}