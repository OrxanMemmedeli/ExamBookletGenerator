using EBC.Core.Constants;
using EBC.Core.Helpers.Authentication;
using EBC.Core.Models.Dtos.Identities.User;
using EBC.Core.Models.ResultModel;
using EBC.Core.Repositories.Concrete;
using EBC.Core.Services.Concrete;
using EBC.Data.Entities;
using EBC.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EBC.Data.Repositories.Concrete;

public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
{
    public AppUserRepository(DbContext context) : base(context)
    {
    }

    public async Task<(Result<UserLoginResponseDTO>, List<Claim>)> GetLoginInfo(string userName, string password)
    {
        var encriptPassword = EncryptionService.Encrypt(EncryptionService.Encrypt(password));

        var user = await base.entity
            .Where(x => x.UserName == userName && x.Password == encriptPassword && x.Status && !x.IsDeleted)
            .Include(i => i.UserRoles)
            .ThenInclude(i => i.Role.OrganizationAdressRoles)
            .ThenInclude(i => i.OrganizationAdress)
            .SingleOrDefaultAsync();

        if (user == null)
            return (Result<UserLoginResponseDTO>.Failure(ExceptionMessage.NotFound), new List<Claim>());
        var userDto = new UserLoginResponseDTO
        {
            UserId = user.Id,
            IsAdmin = user.UserRoles.Select(user => user.Role.Name).Contains(ApplicationCommonField.adminRoleName),
            IsManager = user.UserRoles.Select(user => user.Role.Name).Contains(ApplicationCommonField.managerRoleName),
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FirstName + ' ' + user.LastName,
            LoginTime = DateTime.Now,

            Roles = string.Join(',', user.UserRoles.Select(user => user.Role.Name)),

            Organizations = string.Join(',', user.UserRoles
                .SelectMany(userRole => userRole.Role.OrganizationAdressRoles)
                .Select(orgRole => orgRole.OrganizationAdress)
                .Distinct()
                .Select(org => org.RequestAdress)),

            CompanyIds = string.Join(',' , user.CompanyUsers.Select(i => i.CompanyId))
        };

        return (Result<UserLoginResponseDTO>.Success(userDto), GetClaims(userDto));
    }

    private static List<Claim> GetClaims(UserLoginResponseDTO dto)
    => new List<Claim>
            {
                new Claim(CustomClaimTypes.UserId, dto.UserId.ToString()),
                new Claim(CustomClaimTypes.IsAdmin, dto.IsAdmin.ToString()),
                new Claim(CustomClaimTypes.IsManager, dto.IsManager.ToString()),
                new Claim(CustomClaimTypes.UserName, dto.UserName),
                new Claim(CustomClaimTypes.FirstName, dto.FirstName),
                new Claim(CustomClaimTypes.LastName, dto.LastName),
                new Claim(CustomClaimTypes.FullName, dto.FullName),
                new Claim(CustomClaimTypes.LoginTime, dto.LoginTime.ToString()),

                new Claim(CustomClaimTypes.Roles, dto.Roles),
                new Claim(CustomClaimTypes.OrganizationAddress, dto.Organizations),
                new Claim(CustomClaimTypes.CompanyIds, dto.CompanyIds)
            };
}
