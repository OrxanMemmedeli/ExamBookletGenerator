namespace EBC.Core.Models;

public class ToolsButtonViewModel
{
    public string? Area { get; set; } = default;
    public string? Controller { get; set; } = default;
    public Guid RouteId { get; set; } = default;

    public bool Editable { get; set; } = true;
    public bool Deletable { get; set; } = true;
    public bool Removable { get; set; } = true;
}
