using EBC.Core.Exceptions;
using EBC.Core.IEntities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EBC.Core.SeedData;

/// <summary>
/// Verilənlər bazasına ilkin məlumatları (seed data) əlavə edən sinif.
/// </summary>
public class SeedData
{
    private readonly IOrganizationAdressRepository _organizationAdressRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IOrganizationAdressRoleRepository _organizationAdressRoleRepository;
    private readonly DbContext _dbContext;
    private readonly ISysExceptionRepository _sysExceptionRepository;

    public SeedData(
        IOrganizationAdressRepository organizationAdressRepository,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IOrganizationAdressRoleRepository organizationAdressRoleRepository,
        DbContext dbContext,
        ISysExceptionRepository sysExceptionRepository)
    {
        _organizationAdressRepository = organizationAdressRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _organizationAdressRoleRepository = organizationAdressRoleRepository;
        _dbContext = dbContext;
        _sysExceptionRepository = sysExceptionRepository;
    }

    /// <summary>
    /// Verilənlər bazasına ilkin istifadəçi məlumatlarını əlavə edir.
    /// </summary>
    private List<IUser> GetUsers()
    {
        var adminPass = AncryptionAndDecryption.Encodedata("Aa123!@#");
        var managerPass = AncryptionAndDecryption.Encodedata("Mm123!!");

        return new List<IUser>
        {
            new User
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = true,
                IsDeleted = false,
                FirstName = "SuperAdmin",
                LastName = "SuperAdmin",
                UserName = "admin",
                Password = adminPass
            },
            new User
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = true,
                IsDeleted = false,
                FirstName = "Manager",
                LastName = "Manager",
                UserName = "manager",
                Password = managerPass
            }
        };
    }

    /// <summary>
    /// Verilənlər bazasına ilkin rol məlumatlarını əlavə edir.
    /// </summary>
    private List<IRole> GetRoles()
    {
        return new List<IRole>
        {
            new Role
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = true,
                IsDeleted = false,
                Name = "admin"
            },
            new Role
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = true,
                IsDeleted = false,
                Name = "manager"
            }
        };
    }

    /// <summary>
    /// Verilənlər bazasındakı mövcud təşkilat ünvanlarını əldə edir.
    /// </summary>
    private List<IOrganizationAdress> GetOrganizationAdresses()
    {
        return _organizationAdressRepository.GetAll().GetAwaiter().GetResult().Cast<IOrganizationAdress>().ToList();
    }

    /// <summary>
    /// İlkin məlumatları bazaya yazır.
    /// </summary>
    public async Task WriteSeedData()
    {
        string[] loops = { "admin", "manager" };
        var roles = _roleRepository.GetAll(x => loops.Contains(x.Name)).GetAwaiter().GetResult();

        if (!_userRepository.EntityAny())
        {
            List<IUser> users = GetUsers();
            _userRepository.AddRangeWithoutSave(users);
            List<IRole> roles = GetRoles();
            _roleRepository.AddRangeWithoutSave(roles);

            await AddUserRolesAndOrganizationRoles(users, roles, loops);
        }
        else
        {
            await AddOrganizationRolesOnly(roles, loops);
        }
    }

    private async Task AddUserRolesAndOrganizationRoles(List<IUser> users, List<IRole> roles, string[] loops)
    {
        var organizations = GetOrganizationAdresses();
        using var transaction = _dbContext.Database.BeginTransaction();

        try
        {
            var userRoles = new List<IUserRole>();
            var organizationRoles = new List<IOrganizationAdressRole>();

            foreach (var loop in loops)
            {
                var user = users.FirstOrDefault(u => u.UserName.Equals(loop, StringComparison.OrdinalIgnoreCase));
                var role = roles.FirstOrDefault(r => r.Name.Equals(loop, StringComparison.OrdinalIgnoreCase));

                if (user != null && role != null)
                {
                    userRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
                    var relatedOrgs = loop == "admin" ? organizations : organizations.Where(o => !o.RequestAdress.Contains("Delete", StringComparison.OrdinalIgnoreCase)).ToList();

                    organizationRoles.AddRange(relatedOrgs.Select(o => new OrganizationAdressRole { RoleId = role.Id, OrganizationAdressId = o.Id }));
                }
            }

            _userRoleRepository.AddRangeWithoutSave(userRoles);
            _organizationAdressRoleRepository.AddRangeWithoutSave(organizationRoles);

            transaction.Commit();
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            await WriteError(ex, "SeedData");
            throw;
        }
    }

    private async Task AddOrganizationRolesOnly(List<IRole> roles, string[] loops)
    {
        var organizations = GetOrganizationAdresses();
        var oldOrganizationRoles = _organizationAdressRoleRepository.GetAll().GetAwaiter().GetResult();
        var newOrganizationRoles = new List<IOrganizationAdressRole>();

        try
        {
            foreach (var loop in loops)
            {
                var role = roles.FirstOrDefault(r => r.Name.Equals(loop, StringComparison.OrdinalIgnoreCase));
                if (role == null) continue;

                var existingIds = oldOrganizationRoles.Select(o => o.OrganizationAdressId);
                var relatedOrgs = loop == "admin" ? organizations.Where(o => !existingIds.Contains(o.Id)).ToList() : organizations.Where(o => !o.RequestAdress.Contains("Delete") && !existingIds.Contains(o.Id)).ToList();

                newOrganizationRoles.AddRange(relatedOrgs.Select(o => new OrganizationAdressRole { RoleId = role.Id, OrganizationAdressId = o.Id }));
            }

            _organizationAdressRoleRepository.AddRange(newOrganizationRoles);
        }
        catch (Exception ex)
        {
            await WriteError(ex, "SeedData");
            throw;
        }
    }

    private async Task WriteError(Exception ex, string path)
    {
        var entity = new SysException
        {
            Id = Guid.NewGuid(),
            Exception = ex.ToString(),
            Message = ex.Message,
            RequestPath = path,
            StackTrace = ex.StackTrace,
            UserName = "System",
            UserIP = "system",
            StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnhadleException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };

        await _sysExceptionRepository.AddAsync(entity);
    }
}
