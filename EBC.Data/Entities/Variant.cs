using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class Variant : AuditableEntity<Guid, EBC.Data.Entities.Identity.User>, IAuditable
{
    public Variant()
    {
        Booklets = new HashSet<Booklet>();

    }

    public string Name { get; set; }

    public ICollection<Booklet> Booklets { get; set; }

}
