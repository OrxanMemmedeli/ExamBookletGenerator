namespace EBC.Data.Entities.Identity;

public class OrganizationAdressRole
{
    public Guid OrganizationAdressId { get; set; }
    public Guid RoleId { get; set; }
     
    public OrganizationAdress OrganizationAdress { get; set; }
    public Role Role { get; set; }
}
