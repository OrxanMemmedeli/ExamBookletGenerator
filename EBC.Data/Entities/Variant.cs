using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class Variant : BaseEntity<Guid>
{
    public Variant()
    {
        Booklets = new HashSet<Booklet>();

    }

    public string Name { get; set; }

    public ICollection<Booklet> Booklets { get; set; }

}
