namespace MiniMES.DTOs;

public class ProcessDto
{
    public int Id { get; set; }
    public int SerialNumber { get; set; }
    public string Status { get; set; } = "";
    public DateTime DateTime { get; set; }
    public string Order { get; set; } = "";
}

public class CreateProcessDto
{
    public int SerialNumber { get; set; }
    public int OrderId { get; set; }
    public string Status { get; set; } = null!;
}