using EBC.Core.Repositories.Abstract;
using EBC.Core.Repositories.Concrete;
using EBC.Data.Repositories.Abstract;
using EBC.Data.Repositories.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EBC.Data;

public static class DataServiceRegistration
{
    public static IServiceCollection AddDataLayerServices(this IServiceCollection services)
    {

        // Repositories
        AddRepositoryServices(services);


        // SeedData
        services.AddScoped<EBC.Data.SeedData.SeedData>();

        return services;
    }



    private static void AddRepositoryServices(IServiceCollection services)
    {
        // Generic Repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IGenericRepositoryWithoutBase<>), typeof(GenericRepository<>));

        // Xüsusi repository-ləri qeyd edin (əgər varsa)
        services.AddScoped<IAcademicYearRepository, AcademicYearRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IAuthenticationHistoryRepository, AuthenticationHistoryRepository>();
        services.AddScoped<IBookletRepository, BookletRepository>();
        services.AddScoped<IExamParameterRepository, ExamParameterRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();
        services.AddScoped<IGradeRepository, GradeRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IOrganizationAdressRepository, OrganizationAdressRepository>();
        services.AddScoped<IOrganizationAdressRoleRepository, OrganizationAdressRoleRepository>();
        services.AddScoped<IPaymentOrDebtRepository, PaymentOrDebtRepository>();
        services.AddScoped<IPaymentSummaryRepository, PaymentSummaryRepository>();
        services.AddScoped<IQuestionLevelRepository, QuestionLevelRepository>();
        services.AddScoped<IQuestionParameterRepository, QuestionParameterRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IQuestionTypeRepository, QuestionTypeRepository>();
        services.AddScoped<IResponseRepository, ResponseRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        services.AddScoped<ISendingEmailRepository, SendingEmailRepository>();
        services.AddScoped<ISubjectParameterRepository, SubjectParameterRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<ISysExceptionRepository, SysExceptionRepository>();
        services.AddScoped<ITextRepository, TextRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IUserTypeRepository, UserTypeRepository>();
        services.AddScoped<IVariantRepository, VariantRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();

    }
}
