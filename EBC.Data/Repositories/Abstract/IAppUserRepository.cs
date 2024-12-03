using EBC.Core.Models.Dtos.Identities.User;
using EBC.Core.Models.ResultModel;
using EBC.Core.Repositories.Abstract;
using EBC.Data.Entities;
using System.Security.Claims;

namespace EBC.Data.Repositories.Abstract;

public interface IAppUserRepository : IGenericRepository<AppUser>
{
    Task<(Result<UserLoginResponseDTO>, List<Claim>)> GetLoginInfo(string userName, string password);

}
