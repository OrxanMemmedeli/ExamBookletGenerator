using EBC.Core.Models.ResultModel;
using EBC.Core.Repositories.Abstract;
using EBC.Data.DTOs.Company;
using EBC.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace EBC.Data.Repositories.Abstract;

public interface ICompanyRepository : IGenericRepository<Company>
{
    Task<Result> EditCompanyWithoutPayment(CompanyEditDTO company);
    Task<Result> SaveCompanyWithPaymentSummary(Company company, HttpContext httpContext);
    Task<Result> StartSubscription(Guid id);
    Task<Result> StopSubscription(Guid id);
    List<CompanyCacheViewDTO> GetCompaniesForCache();
    Result<CompanyAccountResponceDTO> GetAccountResponce(CompanyCacheViewDTO company);
    Task<Result> ConfirmMail(string encodeData);
    Task<Result> GenerateNewSecretKey(Guid id);
    Task<CompanyCommonDataDTO> GetCompanyGeneralData(Expression<Func<Company, bool>> predicate);

}
