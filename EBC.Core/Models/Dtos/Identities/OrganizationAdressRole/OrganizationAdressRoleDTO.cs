﻿
namespace EBC.Core.Models.Dtos.Identities.OrganizationAdressRole;

public class OrganizationAdressRoleDTO
{
    public Guid[] Checked { get; set; }
    public List<EBC.Core.Entities.Identity.OrganizationAdress> OrganizationAdresses { get; set; }
    public Guid RoleId { get; set; }
    public Guid[] FormChecked { get; set; }
}
