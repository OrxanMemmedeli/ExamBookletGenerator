using System.ComponentModel.DataAnnotations.Schema;

namespace EBC.Core.Entities.Common;

public abstract class AuditableEntity<TId, TEntity> : BaseEntity<TId>
{
    [ForeignKey("CreateUserId")]
    public TId? CreateUserId { get; set; }
    [ForeignKey("ModifyUserId")]
    public TId? ModifyUserId { get; set; }

    public virtual TEntity? CreateUser { get; set; }
    public virtual TEntity? ModifyUser { get; set; }
}
