using EBC.Core.Entities.Common;
using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace EBC.Data.Configurations;

public class UserConfig : BaseEntityConfig<User, Guid>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.UserType)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.UserTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserTypeId);




        builder.HasMany(x => x.AcademicYears)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.AcademicYearsM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.Grades)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.GradesM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.Questions)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.QuestionsM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.QuestionLevels)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.QuestionLevelsM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.QuestionTypes)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.QuestionTypesM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.Responses)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ResponsesM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.Subjects)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.SubjectsM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.Sections)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.SectionsM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.Exams)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ExamsM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.SubjectParameters)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.SubjectParametersM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.ExamParameters)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ExamParametersM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.Texts)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.TextsM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.Variants)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.VariantsM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.QuestionParameters)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.QuestionParametersM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.Attachments)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.AttachmentsM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.Groups)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.GroupsM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(x => x.Booklets)
            .WithOne(x => x.CreateUser)
            .HasForeignKey(x => x.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.BookletsM)
            .WithOne(x => x.ModifyUser)
            .HasForeignKey(x => x.ModifyUserId)
            .OnDelete(DeleteBehavior.Restrict);

    }

    private void ConfigureFieldForCreate<T>(
        EntityTypeBuilder<User> builder,
        Expression<Func<User, IEnumerable<T>>> hasMany,
        Expression<Func<T, User>> withOne,
        Expression<Func<T, object>> hasForeignKey) where T : Entity<Guid>
    {
        builder.HasMany(hasMany)
            .WithOne(withOne)
            .HasForeignKey(hasForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
