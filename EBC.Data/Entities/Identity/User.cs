using EBC.Core.Entities.Common;
using EBC.Data.Entities.CombineEntities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBC.Data.Entities.Identity;

public class User : BaseEntity<Guid>
{
    public User()
    {
        Grades = new HashSet<Grade>();
        AcademicYears = new HashSet<AcademicYear>();
        QuestionLevels = new HashSet<QuestionLevel>();
        Questions = new HashSet<Question>();
        QuestionTypes = new HashSet<QuestionType>();
        Responses = new HashSet<Response>();
        Subjects = new HashSet<Subject>();
        Sections = new HashSet<Section>();
        Exams = new HashSet<Exam>();
        SubjectParameters = new HashSet<SubjectParameter>();
        ExamParameters = new HashSet<ExamParameter>();
        Texts = new HashSet<Text>();
        Variants = new HashSet<Variant>();
        QuestionParameters = new HashSet<QuestionParameter>();
        Booklets = new HashSet<Booklet>();
        Groups = new HashSet<Group>();
        Attachments = new HashSet<Attachment>();

        GradesM = new HashSet<Grade>();
        AcademicYearsM = new HashSet<AcademicYear>();
        QuestionLevelsM = new HashSet<QuestionLevel>();
        QuestionsM = new HashSet<Question>();
        QuestionTypesM = new HashSet<QuestionType>();
        ResponsesM = new HashSet<Response>();
        SubjectsM = new HashSet<Subject>();
        SectionsM = new HashSet<Section>();
        ExamsM = new HashSet<Exam>();
        SubjectParametersM = new HashSet<SubjectParameter>();
        ExamParametersM = new HashSet<ExamParameter>();
        TextsM = new HashSet<Text>();
        VariantsM = new HashSet<Variant>();
        QuestionParametersM = new HashSet<QuestionParameter>();
        BookletsM = new HashSet<Booklet>();
        GroupsM = new HashSet<Group>();
        AttachmentsM = new HashSet<Attachment>();


        CompanyUsers = new HashSet<CompanyUser>();

    }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public string ImagePath { get; set; }


    #region Referances
    public Guid? UserTypeId { get; set; }

    public virtual UserType? UserType { get; set; }
    #endregion


    #region Collections
    public ICollection<Grade> Grades { get; set; }
    public ICollection<AcademicYear> AcademicYears { get; set; }
    public ICollection<Question> Questions { get; set; }
    public ICollection<QuestionLevel> QuestionLevels { get; set; }
    public ICollection<QuestionType> QuestionTypes { get; set; }
    public ICollection<Response> Responses { get; set; }
    public ICollection<Subject> Subjects { get; set; }
    public ICollection<Section> Sections { get; set; }
    public ICollection<Exam> Exams { get; set; }
    public ICollection<SubjectParameter> SubjectParameters { get; set; }
    public ICollection<ExamParameter> ExamParameters { get; set; }
    public ICollection<Text> Texts { get; set; }
    public ICollection<Variant> Variants { get; set; }
    public ICollection<QuestionParameter> QuestionParameters { get; set; }
    public ICollection<Booklet> Booklets { get; set; }
    public ICollection<Group> Groups { get; set; }
    public ICollection<Attachment> Attachments { get; set; }


    public ICollection<Grade> GradesM { get; set; }
    public ICollection<AcademicYear> AcademicYearsM { get; set; }
    public ICollection<Question> QuestionsM { get; set; }
    public ICollection<QuestionLevel> QuestionLevelsM { get; set; }
    public ICollection<QuestionType> QuestionTypesM { get; set; }
    public ICollection<Response> ResponsesM { get; set; }
    public ICollection<Subject> SubjectsM { get; set; }
    public ICollection<Section> SectionsM { get; set; }
    public ICollection<Exam> ExamsM { get; set; }
    public ICollection<SubjectParameter> SubjectParametersM { get; set; }
    public ICollection<ExamParameter> ExamParametersM { get; set; }
    public ICollection<Text> TextsM { get; set; }
    public ICollection<Variant> VariantsM { get; set; }
    public ICollection<QuestionParameter> QuestionParametersM { get; set; }
    public ICollection<Booklet> BookletsM { get; set; }
    public ICollection<Group> GroupsM { get; set; }
    public ICollection<Attachment> AttachmentsM { get; set; }


    public ICollection<CompanyUser> CompanyUsers { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }

    #endregion


}
