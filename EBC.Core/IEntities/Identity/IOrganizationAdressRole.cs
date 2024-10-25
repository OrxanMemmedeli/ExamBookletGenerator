namespace EBC.Core.IEntities.Identity;

public interface IOrganizationAdressRole
{
    public Guid OrganizationAdressId { get; set; }
    public Guid RoleId { get; set; }
     
    public IOrganizationAdress OrganizationAdress { get; set; }
    public IRole Role { get; set; }
}
