using EBC.Core.Exceptions;
using EBC.Core.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using EBC.Core.Repositories.Abstract;
using EBC.Core.Services.Concrete;
using EBC.Core.Constants;
using EBC.Core.Entities;
using EBC.Data.Repositories.Abstract;
using EBC.Data.Entities;

namespace EBC.Data.SeedData;

/// <summary>
/// Verilənlər bazasına ilkin məlumatları (seed data) əlavə edən sinif.
/// </summary>
/// <remarks>
/// Bu sinif, tətbiqin işləməsi üçün lazım olan ilkin məlumatları, məsələn, istifadəçi, rol və təşkilat ünvanlarını verilənlər bazasına əlavə etmək üçün istifadə olunur.
/// </remarks>
public class SeedData
{
    private readonly IOrganizationAdressRepository _organizationAdressRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IOrganizationAdressRoleRepository _organizationAdressRoleRepository;
    private readonly DbContext _dbContext;
    private readonly ISysExceptionRepository _sysExceptionRepository;


    /// <summary>
    /// SeedData sinifini yaradır və asılılıqda olan repository-ləri inject edir.
    /// </summary>
    public SeedData(
        IOrganizationAdressRepository organizationAdressRepository,
        IAppUserRepository appUserRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IOrganizationAdressRoleRepository organizationAdressRoleRepository,
        DbContext dbContext,
        ISysExceptionRepository sysExceptionRepository)
    {
        _organizationAdressRepository = organizationAdressRepository;
        _appUserRepository = appUserRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _organizationAdressRoleRepository = organizationAdressRoleRepository;
        _dbContext = dbContext;
        _sysExceptionRepository = sysExceptionRepository;
    }

    /// <summary>
    /// Verilənlər bazasına ilkin məlumatları əlavə edir.
    /// </summary>
    /// <remarks>
    /// İlk olaraq, verilənlər bazasında istifadəçi olub-olmadığını yoxlayır. Əgər istifadəçi yoxdursa, default olaraq admin və menecer istifadəçi və rol məlumatlarını əlavə edir.
    /// Mövcud olduqda isə yalnız təşkilat rollarını yeniləyir.
    /// </remarks>
    public async Task WriteSeedDataAsync()
    {
        string[] roleNames = { ApplicationCommonField.adminRoleName, ApplicationCommonField.managerRoleName };

        if (!_appUserRepository.EntityAny())
        {
            var users = GetDefaultUsers();
            var roles = GetDefaultRoles();

            _appUserRepository.AddRangeWithoutSave(users);
            _roleRepository.AddRangeWithoutSave(roles);

            await AddUserAndOrganizationRoles(users, roles, roleNames, includeUserRoles: true);
        }
        else
        {
            var existingRoles = await _roleRepository.GetAll(x => roleNames.Contains(x.Name));
            await AddUserAndOrganizationRoles(null, existingRoles, roleNames, includeUserRoles: false);
        }
    }


    /// <summary>
    /// İstifadəçi və təşkilat rollarını əlavə edir.
    /// </summary>
    /// <param name="users">Əlavə ediləcək istifadəçilər.</param>
    /// <param name="roles">Əlavə ediləcək rollar.</param>
    /// <param name="roleNames">Rol adları.</param>
    /// <param name="includeUserRoles">Əgər true dəyəri verilsə, istifadəçi rolları da əlavə olunur.</param>
    /// <remarks>
    /// Bu metod həm istifadəçilərə, həm də təşkilat ünvanlarına uyğun rolları təyin edir və əlavə edir. 
    /// Verilənlərin hamısı bir transaksiyada yerinə yetirilir və hər hansı bir xətada bütün əməliyyatlar geri çevrilir.
    /// </remarks>
    private async Task AddUserAndOrganizationRoles(List<AppUser>? users, List<Role> roles, string[] roleNames, bool includeUserRoles)
    {
        var organizations = GetOrganizationAdresses();
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var userRoles = new List<UserRole>();
            var organizationRoles = new List<OrganizationAdressRole>();

            foreach (var roleName in roleNames)
            {
                var role = roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
                if (role == null) continue;

                if (includeUserRoles && users != null)
                {
                    var user = users.FirstOrDefault(u => u.UserName.Equals(roleName, StringComparison.OrdinalIgnoreCase));
                    if (user != null)
                        userRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
                }

                var relevantOrgs = roleName == ApplicationCommonField.adminRoleName
                    ? organizations
                    : organizations.Where(o => !o.RequestAdress.Contains(ApplicationCommonField.delete, StringComparison.OrdinalIgnoreCase)).ToList();

                organizationRoles.AddRange(relevantOrgs.Select(o => new OrganizationAdressRole
                {
                    RoleId = role.Id,
                    OrganizationAdressId = o.Id
                }));
            }

            if (includeUserRoles) _userRoleRepository.AddRangeWithoutSave(userRoles);
            _organizationAdressRoleRepository.AddRangeWithoutSave(organizationRoles);

            await transaction.CommitAsync();
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            await LogError(ex, ApplicationCommonField.seedData);
        }
    }

    /// <summary>
    /// Varsayılan istifadəçiləri gətirir.
    /// </summary>
    /// <returns>İstifadəçilərin siyahısını döndərir.</returns>
    /// <remarks>
    /// Bu metod default admin və menecer istifadəçi hesablarını gətirir və hər bir hesab üçün unikal ID və şifrə təyin edir.
    /// </remarks>
    private List<AppUser> GetDefaultUsers()
    {
        return new List<AppUser>
        {
            new AppUser
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = true,
                IsDeleted = false,
                FirstName = ApplicationCommonField.adminFirstName,
                LastName = ApplicationCommonField.adminLastName,
                UserName = ApplicationCommonField.adminUserName,
                Password = EncryptionService.Encrypt(ApplicationCommonField.adminPass)
            },
            new AppUser
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = true,
                IsDeleted = false,
                FirstName = ApplicationCommonField.managerFirstName,
                LastName = ApplicationCommonField.managerLastName,
                UserName = ApplicationCommonField.managerUserName,
                Password = EncryptionService.Encrypt(ApplicationCommonField.managerPass)
            }
        };
    }

    /// <summary>
    /// Varsayılan rolları gətirir.
    /// </summary>
    /// <returns>Rolların siyahısını döndərir.</returns>
    /// <remarks>
    /// Default olaraq tətbiqdə admin və menecer rollarını döndərir.
    /// </remarks>
    private List<Role> GetDefaultRoles()
    {
        return new List<Role>
        {
            new Role
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = true,
                IsDeleted = false,
                Name = ApplicationCommonField.adminRoleName
            },
            new Role
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Status = true,
                IsDeleted = false,
                Name = ApplicationCommonField.managerRoleName
            }
        };
    }

    /// <summary>
    /// Təşkilat ünvanlarını gətirir.
    /// </summary>
    /// <returns>Təşkilat ünvanlarının siyahısını döndərir.</returns>
    private List<OrganizationAdress> GetOrganizationAdresses()
        => _organizationAdressRepository.GetAll().GetAwaiter().GetResult().ToList();

    /// <summary>
    /// Xətanı qeyd edir.
    /// </summary>
    /// <param name="ex">Xəta məlumatı.</param>
    /// <param name="source">Xətanın baş verdiyi yerin adı.</param>
    /// <remarks>
    /// Bu metod yaranan xətaları verilənlər bazasına qeyd edir və əlavə məlumatlar verir.
    /// </remarks>
    private async Task LogError(Exception ex, string source)
    {
        var sysException = new SysException
        {
            Id = Guid.NewGuid(),
            Exception = ex.ToString(),
            Message = ex.Message,
            RequestPath = source,
            StackTrace = ex.StackTrace,
            UserName = ApplicationCommonField.system,
            UserIP = ApplicationCommonField.systemIP,
            StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnhadleException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };

        await _sysExceptionRepository.AddAsync(sysException);
    }
}
