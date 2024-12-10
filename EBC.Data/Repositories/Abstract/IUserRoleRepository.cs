using EBC.Core.Repositories.Abstract;
using EBC.Data.DTOs.Identities.UserRole;
using EBC.Data.Entities.Identity;

namespace EBC.Data.Repositories.Abstract;

public interface IUserRoleRepository : IGenericRepositoryWithoutBase<UserRole>
{
    UserRoleDTO GetCustomData(Guid userId, List<Role> roles);

    Task<int> UpdateForUser(UserRoleDTO model);
}
