using EBC.Core.Models.Commons;

namespace EBC.Data.DTOs.Text;

public class TextCreateDTO : BaseEntityCreateDTO
{
    public string Name { get; set; }
    public string Title { get; set; } = "Mətni oxuyun və {0} – {1} nömrəli tapşırıqları mətnə uyğun cavablayın.";
    public string Content { get; set; }
}
