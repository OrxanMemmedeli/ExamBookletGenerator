using EBC.Core.Constants;
using EBC.Core.Entities.Identity;
using EBC.Core.Models.Dtos.Identities.User;
using EBC.Core.Models.ResultModel;
using EBC.Core.Repositories.Abstract;
using EBC.Core.Services.Concrete;
using Microsoft.EntityFrameworkCore;

namespace EBC.Core.Repositories.Concrete;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }
    public Task<Result> AddUser(User entity)
    {
        if (base.entity.Any(x => x.UserName == entity.UserName))
            return Task.FromResult<Result>(Result.Failure(ExceptionMessage.UniqueUser));

        entity.Password = EncryptionService.Encrypt(EncryptionService.Encrypt(entity.Password));

        base.entity.Add(entity);
        base.SaveChanges();

        return Task.FromResult<Result>(Result.Success());
    }

    public async Task<Result<UserLoginResponseDTO>> GetLoginInfo(string userName, string password)
    {
        var encriptPassword = EncryptionService.Encrypt(EncryptionService.Encrypt(password));

        var user = await base.entity
            .Where(x => x.UserName == userName && x.Password == encriptPassword && x.Status && !x.IsDeleted)
            .Include(i => i.UserRoles)
            .ThenInclude(i => i.Role.OrganizationAdressRoles)
            .ThenInclude(i => i.OrganizationAdress)
            .SingleOrDefaultAsync();

        if (user == null)
            return Result<UserLoginResponseDTO>.Failure(ExceptionMessage.NotFound);

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


            Roles = string.Join(", ", user.UserRoles.Select(user => user.Role.Name)),

            Organizations = string.Join(", ", user.UserRoles
                .SelectMany(userRole => userRole.Role.OrganizationAdressRoles)
                .Select(orgRole => orgRole.OrganizationAdress)
                .Distinct()
                .Select(org => org.RequestAdress))
        };

        return Result<UserLoginResponseDTO>.Success(userDto);

    }

    public Task<Result> UpdateUser(Guid userId, UserEditDTO dto)
    {
        var user = base.entity.FirstOrDefault(x => x.Id == userId);

        if (base.entity.Any(x => x.UserName == dto.UserName && x.Id != userId))
            return Task.FromResult<Result>(Result.Failure(ExceptionMessage.UniqueUser));

        user.UserName = dto.UserName;
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;

        base.entity.Update(user);
        base.SaveChanges();

        return Task.FromResult<Result>(Result.Success());
    }

    public Task<Result> UpdateUserPassword(UserPasswordEditDTO entity)
    {
        var user = base.entity.FirstOrDefault(x => x.Id == entity.Id);

        if (user == null)
            return Task.FromResult<Result>(Result.Failure(ExceptionMessage.UniqueUser));

        entity.Password = EncryptionService.Decrypt(EncryptionService.Decrypt(entity.Password));

        base.entity.Update(user);
        base.SaveChanges();

        return Task.FromResult<Result>(Result.Success());
    }

}
