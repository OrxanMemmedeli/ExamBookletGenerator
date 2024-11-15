using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Text;

public class TextIndexDTO : BaseEntityViewDTO
{
    public string Name { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}
