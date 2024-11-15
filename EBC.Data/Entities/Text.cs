using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class Text : BaseEntity<Guid>
{
    public Text()
    {
        Questions = new HashSet<Question>();
    }

    public string Name { get; set; }
    public string Title { get; set; } = "Mətni oxuyun və {0} – {1} nömrəli tapşırıqları mətnə uyğun cavablayın.";
    public string Content { get; set; }

    public ICollection<Question> Questions { get; set; }
}
