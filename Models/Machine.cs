using System.ComponentModel.DataAnnotations;

namespace MiniMES.Models;

public class Machine
{
    [Key]
    public int Id {get; set;}
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}