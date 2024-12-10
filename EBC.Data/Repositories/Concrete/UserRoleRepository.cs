using EBC.Core.Repositories.Concrete;
using EBC.Data.DTOs.Identities.UserRole;
using EBC.Data.Entities.Identity;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Data.Repositories.Concrete;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(DbContext context) : base(context)
    {
    }

    public UserRoleDTO GetCustomData(Guid userId, List<Role> roles)
    {
        var checkeds = base.entity.Where(x => x.UserId == userId).Select(x => x.RoleId).ToArray();
        return new UserRoleDTO()
        {
            Checked = checkeds,
            Roles = roles,
            UserId = userId,
            FormChecked = null
        };
    }

    public async Task<int> UpdateForUser(UserRoleDTO model)
    {
        Guid[] newList = (model.FormChecked ?? Enumerable.Empty<Guid>()).Except(model.Checked ?? Enumerable.Empty<Guid>()).ToArray();
        Guid[] oldList = (model.Checked ?? Enumerable.Empty<Guid>()).Except(model.FormChecked ?? Enumerable.Empty<Guid>()).ToArray();

        IEnumerable<UserRole> listForAdd = newList?
            .Select(role => new UserRole { UserId = model.UserId, RoleId = role })
            ?? new List<UserRole>();

        IEnumerable<UserRole> listForDelete = oldList?
            .Select(role => new UserRole { UserId = model.UserId, RoleId = role })
            ?? new List<UserRole>();

        AddRangeWithoutSave(listForAdd);
        DeleteRangeWithoutSave(listForDelete);
        return await SaveChangesAsync();
    }
}
