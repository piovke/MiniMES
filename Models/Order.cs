using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniMES.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    public string Code { get; set; } = "";
    public int MachineId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    [ForeignKey("MachineId")]
    public Machine Machine { get; set; } = null!;
    [ForeignKey("ProductId")]
    public Product Product { get; set; } = null!;
    
    public ICollection<Process> Processes { get; set; } = new List<Process>();
}