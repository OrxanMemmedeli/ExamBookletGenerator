using EBC.Core.Models.ResultModel;
using EBC.Core.Repositories.Abstract;
using EBC.Data.DTOs.Identities.User;
using EBC.Data.Entities.Identity;
using System.Security.Claims;

namespace EBC.Data.Repositories.Abstract;

public interface IUserRepository : IGenericRepository<User>
{
    Task<(Result<UserLoginResponseDTO>, List<Claim>)> GetLoginInfo(string userName, string password);
    Task<Result> AddUser(User entity);
    Task<Result> UpdateUser(Guid userId, UserEditDTO dto);
    Task<Result> UpdateUserPassword(UserPasswordEditDTO entity);
}
