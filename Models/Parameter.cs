namespace MiniMES.Models;

public class Parameter
{
    public int Id;
    public string Name;
    public string Unit;
    
    public ICollection<ProcessParameters> ProcessParameters { get; set; } = new List<ProcessParameters>();
}