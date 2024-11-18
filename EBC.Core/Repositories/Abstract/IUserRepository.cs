using AccountManagerSystem.Repositories.Abstract;
using EBC.Core.Entities.Identity;
using EBC.Core.Models.Dtos.Identities.User;
using EBC.Core.Models.ResultModel;

namespace EBC.Core.Repositories.Abstract;

public interface IUserRepository : IGenericRepository<User>
{
    Task<Result> AddUser(User entity);
    Task<Result<UserLoginResponseDTO>> GetLoginInfo(string userName, string password);
    Task<Result> UpdateUser(Guid userId, UserEditDTO dto);
    Task<Result> UpdateUserPassword(UserPasswordEditDTO entity);
}
