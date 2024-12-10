using EBC.Core.Repositories.Abstract;
using EBC.Data.DTOs.Identities.OrganizationAdressRole;
using EBC.Data.Entities.Identity;

namespace EBC.Data.Repositories.Abstract;

public interface IOrganizationAdressRoleRepository : IGenericRepositoryWithoutBase<OrganizationAdressRole>
{
    OrganizationAdressRoleDTO GetCustomData(Guid roleId, List<OrganizationAdress> organizations);
    Task<int> UpdateForRole(OrganizationAdressRoleDTO model);
}
