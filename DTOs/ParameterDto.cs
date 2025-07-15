namespace MiniMES.DTOs;

public class ParameterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Unit { get; set; } = "";
    // public List<
}

public class CreateParameterDto
{
    public string Name { get; set; } = "";
    public string Unit { get; set; } = "";
}