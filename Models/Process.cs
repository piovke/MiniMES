namespace MiniMES.Models;

public class Process
{
    public int Id { get; set; }
    public int SerialNumber { get; set; }
    public int OrderId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime DateTime { get; set; }
    
    public Order Order { get; set; } = null!;
    
    public ICollection<ProcessParameters> ProcessParameters { get; set; } = null!;
}