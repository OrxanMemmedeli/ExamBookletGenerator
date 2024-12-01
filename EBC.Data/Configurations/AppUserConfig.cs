using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations;

public class AppUserConfig : BaseEntityConfig<AppUser, Guid>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.UserType)
            .WithMany(x => x.AppUsers)
            .HasForeignKey(x => x.UserTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserTypeId);

        #region ConfigureField

        // Burada xüsusi sahə konfiqurasiyalarını çağırırıq
        ConfigureField(builder, nameof(AppUser.AcademicYears), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.AcademicYearsM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.Grades), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.GradesM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.Questions), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.QuestionsM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.QuestionLevels), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.QuestionLevelsM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.QuestionTypes), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.QuestionTypesM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.Responses), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.ResponsesM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.Subjects), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.SubjectsM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.Sections), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.SectionsM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.Exams), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.ExamsM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.SubjectParameters), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.SubjectParametersM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.ExamParameters), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.ExamParametersM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.Texts), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.TextsM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.Variants), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.VariantsM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.QuestionParameters), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.QuestionParametersM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.Attachments), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.AttachmentsM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.Groups), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.GroupsM), "ModifyUser", "ModifiedUserId");

        ConfigureField(builder, nameof(AppUser.Booklets), "CreatUser", "CreatedUserId");
        ConfigureField(builder, nameof(AppUser.BookletsM), "ModifyUser", "ModifiedUserId");

        #endregion
    }

    private void ConfigureField(EntityTypeBuilder<AppUser> builder, string collectionName, string navigationName, string foreignKeyName)
    {
        var propertyInfo = typeof(AppUser).GetProperty(collectionName);
        if (propertyInfo == null)
            throw new InvalidOperationException($"Property '{collectionName}' not found in AppUser.");

        var collectionType = propertyInfo.PropertyType.GetGenericArguments().FirstOrDefault();
        if (collectionType == null)
            throw new InvalidOperationException($"Invalid collection type for property '{collectionName}' in AppUser.");

        builder.HasMany(collectionType, collectionName)
            .WithOne(navigationName)
            .HasForeignKey(foreignKeyName)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(foreignKeyName);
    }
}
