using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class Group : BaseEntity<Guid>, IAuditable
{

    public Group()
    {
        Booklets = new HashSet<Booklet>();
    }

    public string Name { get; set; }

    public ICollection<Booklet> Booklets { get; set; }

}
