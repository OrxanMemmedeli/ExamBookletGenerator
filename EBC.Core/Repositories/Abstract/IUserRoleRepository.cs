using EBC.Core.Entities.Identity;
using EBC.Core.Models.Dtos.Identities.UserRole;

namespace EBC.Core.Repositories.Abstract;

public interface IUserRoleRepository : IGenericRepositoryWithoutBase<UserRole>
{
    UserRoleDTO GetCustomData(Guid userId, List<Role> roles);

    Task<int> UpdateForUser(UserRoleDTO model);
}
