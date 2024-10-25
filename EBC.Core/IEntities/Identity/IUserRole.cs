namespace EBC.Core.IEntities.Identity;

public interface IUserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
    public IUser User { get; set; }
    public IRole Role { get; set; }
}
