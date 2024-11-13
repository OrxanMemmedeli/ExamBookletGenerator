using EBC.Core.Entities.Identity;
using EBC.Core.Models.Dtos.Identities.OrganizationAdressRole;
using EBC.Core.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EBC.Core.Repositories.Concrete;

public class OrganizationAdressRoleRepository : GenericRepository<OrganizationAdressRole>, IOrganizationAdressRoleRepository
{
    public OrganizationAdressRoleRepository(DbContext context) : base(context)
    {
    }

    public OrganizationAdressRoleDTO GetCustomData(Guid roleId, List<OrganizationAdress> organizations)
    {
        var checkeds = base.entity.Where(x => x.RoleId == roleId).Select(x => x.OrganizationAdressId).ToArray();
        return new OrganizationAdressRoleDTO
        {
            Checked = checkeds,
            OrganizationAdresses = organizations,
            RoleId = roleId,
            FormChecked = null
        };
    }

    public async Task<int> UpdateForRole(OrganizationAdressRoleDTO model)
    {
        Guid[] newList = (model.FormChecked ?? Enumerable.Empty<Guid>()).Except(model.Checked ?? Enumerable.Empty<Guid>()).ToArray();
        Guid[] oldList = (model.Checked ?? Enumerable.Empty<Guid>()).Except(model.FormChecked ?? Enumerable.Empty<Guid>()).ToArray();

        IEnumerable<OrganizationAdressRole> listForAdd = newList?
            .Select(organizationRole => new OrganizationAdressRole { RoleId = model.RoleId, OrganizationAdressId = organizationRole })
            ?? new List<OrganizationAdressRole>();

        IEnumerable<OrganizationAdressRole> listForDelete = oldList?
            .Select(organizationRole => new OrganizationAdressRole { RoleId = model.RoleId, OrganizationAdressId = organizationRole })
            ?? new List<OrganizationAdressRole>();

        AddRangeWithoutSave(listForAdd);
        DeleteRangeWithoutSave(listForDelete);
        return await SaveChangesAsync();

    }

}
