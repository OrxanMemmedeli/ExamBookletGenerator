using AutoMapper;
using EBC.Data.DTOs.AcademicYear;
using EBC.Data.DTOs.AppUser;
using EBC.Data.DTOs.Booklet;
using EBC.Data.DTOs.Company;
using EBC.Data.DTOs.Exam;
using EBC.Data.DTOs.ExamParameter;
using EBC.Data.DTOs.Grade;
using EBC.Data.DTOs.Group;
using EBC.Data.DTOs.Payment;
using EBC.Data.DTOs.PaymentSummary;
using EBC.Data.DTOs.Question;
using EBC.Data.DTOs.QuestionLevel;
using EBC.Data.DTOs.QuestionType;
using EBC.Data.DTOs.Response;
using EBC.Data.DTOs.Section;
using EBC.Data.DTOs.Subject;
using EBC.Data.DTOs.SubjectParameter;
using EBC.Data.DTOs.Text;
using EBC.Data.DTOs.UserType;
using EBC.Data.DTOs.Variant;
using EBC.Data.Entities;
using EBC.Data.Entities.ExceptionalEntities;

namespace EBC.Data.Mappers.AutoMapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        #region AcademicYearProfile
        CreateMap<AcademicYear, AcademicYearViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ReverseMap();

        CreateMap<AcademicYear, AcademicYearCreateDTO>().ReverseMap();
        CreateMap<AcademicYear, AcademicYearEditDTO>().ReverseMap();
        #endregion


        #region AppUserProfile
        CreateMap<AppUser, AcademicYearViewDTO>().ReverseMap();
        CreateMap<AppUser, AppUserCreateEditDTO>().ReverseMap();
        #endregion


        #region BookletProfile
        CreateMap<Booklet, BookletViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ForMember(dest => dest.ExamName, opt => opt.MapFrom(src => src.Exam.Name))
            .ForMember(dest => dest.AcademicYearName, opt => opt.MapFrom(src => src.AcademicYear.Name))
            .ForMember(dest => dest.VariantName, opt => opt.MapFrom(src => src.Variant.Name))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name))
            .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.Grade.Name))
            .ReverseMap();

        CreateMap<Booklet, BookletCreateDTO>().ReverseMap();
        CreateMap<Booklet, BookletEditDTO>().ReverseMap();
        #endregion


        #region CompanyProfile
        CreateMap<Company, CompanyViewDTO>().ReverseMap();
        CreateMap<Company, CompanyCreateDTO>().ReverseMap();
        CreateMap<Company, CompanyEditDTO>().ReverseMap();
        #endregion


        #region ExamParameterProfile
        CreateMap<ExamParameter, ExamParameterViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ReverseMap();

        CreateMap<ExamParameter, ExamParameterCreateDTO>().ReverseMap();
        CreateMap<ExamParameter, ExamParameterEditDTO>().ReverseMap();
        #endregion


        #region ExamProfile
        CreateMap<Exam, ExamViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.Grade.Name))
            .ForMember(dest => dest.ExamParameterName, opt => opt.MapFrom(src => src.ExamParameter.Name))
            .ReverseMap();

        CreateMap<Exam, ExamCreateDTO>().ReverseMap();
        CreateMap<Exam, ExamEditDTO>().ReverseMap();
        #endregion


        #region GradeProfile
        CreateMap<Grade, GradeViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ReverseMap();

        CreateMap<Grade, GradeCreateDTO>().ReverseMap();
        CreateMap<GradeEditDTO, Grade>()
            .ForMember(dest => dest.Name, opt => opt.Condition((src, dest, srcMember) => srcMember != null))
            .ReverseMap();
        #endregion


        #region GroupProfile
        CreateMap<Group, GroupViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ReverseMap();

        CreateMap<Group, GroupCreateDTO>().ReverseMap();
        CreateMap<Group, GroupEditDTO>().ReverseMap();
        #endregion


        #region PaymentProfile
        CreateMap<Payment, PaymentCreateDTO>().ReverseMap();
        #endregion


        #region PaymentSummaryProfile
        CreateMap<PaymentSummary, PaymentSummaryCreateDTO>().ReverseMap();
        CreateMap<PaymentSummary, PaymentSummaryUpdateDTO>().ReverseMap();
        #endregion


        #region QuestionLevelProfile
        CreateMap<QuestionLevel, QuestionLevelViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ReverseMap();

        CreateMap<QuestionLevel, QuestionLevelCreateDTO>().ReverseMap();
        CreateMap<QuestionLevel, QuestionLevelEditDTO>().ReverseMap();
        #endregion


        #region QuestionProfile
        CreateMap<Question, QuestionViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
            .ForMember(dest => dest.SectionName, opt => opt.MapFrom(src => src.Section.Name))
            .ForMember(dest => dest.QuestionLevelName, opt => opt.MapFrom(src => src.QuestionLevel.Name))
            .ForMember(dest => dest.QuestionTypeName, opt => opt.MapFrom(src => src.QuestionType.ResponseType))
            .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.Grade.Name))
            .ForMember(dest => dest.AcademicYearName, opt => opt.MapFrom(src => src.AcademicYear.Name))
            .ReverseMap();

        CreateMap<Question, QuestionCreateDTO>().ReverseMap();
        CreateMap<Question, QuestionEditDTO>().ReverseMap();
        #endregion


        #region QuestionTypeProfile
        CreateMap<QuestionType, QuestionTypeViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ReverseMap();

        CreateMap<QuestionType, QuestionTypeCreateDTO>().ReverseMap();
        CreateMap<QuestionType, QuestionTypeEditDTO>().ReverseMap();
        #endregion


        #region ResponseProfile
        CreateMap<Response, ResponseViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
            .ForMember(dest => dest.QuestionTypeName, opt => opt.MapFrom(src => src.QuestionType.ResponseType))
            .ForMember(dest => dest.QuestionName, opt => opt.MapFrom(src => src.Question.Content))
            .ForMember(dest => dest.AcademicYearName, opt => opt.MapFrom(src => src.AcademicYear.Name))
            .ReverseMap();

        CreateMap<Response, ResponseCreateDTO>().ReverseMap();
        CreateMap<Response, ResponseEditDTO>().ReverseMap();
        #endregion


        #region SectionProfile
        CreateMap<Section, SectionViewDTO>()
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ReverseMap();

        CreateMap<Section, SectionCreateDTO>().ReverseMap();
        CreateMap<Section, SectionEditDTO>().ReverseMap();
        #endregion


        #region SubjectParameterProfile
        CreateMap<SubjectParameter, SubjectParameterViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
            .ForMember(dest => dest.ExamParameterName, opt => opt.MapFrom(src => src.ExamParameter.Name))
            .ReverseMap();

        CreateMap<SubjectParameter, SubjectParameterCreateDTO>().ReverseMap();
        CreateMap<SubjectParameter, SubjectParameterEditDTO>().ReverseMap();
        #endregion


        #region SubjectProfile
        CreateMap<Subject, SubjectViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ReverseMap();

        CreateMap<Subject, SubjectCreateDTO>().ReverseMap();
        CreateMap<Subject, SubjectEditDTO>().ReverseMap();
        #endregion


        #region TextProfile
        CreateMap<Text, TextViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ReverseMap();

        CreateMap<Text, TextCreateDTO>().ReverseMap();
        CreateMap<Text, TextEditDTO>().ReverseMap();
        #endregion


        #region UserTypeProfile
        CreateMap<UserType, UserTypeViewDTO>().ReverseMap();

        CreateMap<UserType, UserTypeCreateDTO>().ReverseMap();
        CreateMap<UserType, UserTypeEditDTO>().ReverseMap();
        #endregion


        #region VariantProfile
        CreateMap<Variant, VariantViewDTO>()
            .MapAuditableFields(mapCreatedUser: false, mapModifiedUser: true)
            .ReverseMap();

        CreateMap<Variant, VariantCreateDTO>().ReverseMap();
        CreateMap<Variant, VariantEditDTO>().ReverseMap();
        #endregion
    }
}
