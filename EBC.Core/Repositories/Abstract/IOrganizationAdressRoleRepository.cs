using EBC.Core.Entities.Identity;
using EBC.Core.Models.Dtos.Identities.OrganizationAdressRole;

namespace EBC.Core.Repositories.Abstract;

public interface IOrganizationAdressRoleRepository : IGenericRepositoryWithoutBase<OrganizationAdressRole>
{
    OrganizationAdressRoleDTO GetCustomData(Guid roleId, List<OrganizationAdress> organizations);
    Task<int> UpdateForRole(OrganizationAdressRoleDTO model);
}
