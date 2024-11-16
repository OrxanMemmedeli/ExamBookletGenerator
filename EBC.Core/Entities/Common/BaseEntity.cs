namespace EBC.Core.Entities.Common;

public abstract class BaseEntity<T> : Entity<T>
{
    public bool Status { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
