using EBC.Core.Entities.Common;
using EBC.Core.Entities.Configurations.Common;
using EBC.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Data.Configurations.Base;


public class AuditableEntityConfig<Tid, T> : IEntityTypeConfiguration<T> where T : AuditableEntity<Tid, User>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasIndex(x => x.CreateUserId);
        builder.HasIndex(x => x.ModifyUserId);

        // Global Filter query əlavəsi üçün. ancaq soft olaraq silinmemis deyerleri getirecek(legv etmek ucun isse repo icinde .IgnoreQueryFilters() methodunu ist elemek lazimdir)
        builder.HasQueryFilter(x => !x.IsDeleted);

        // IsDeleted=false olan qeydlər üzrə indeks yaradır və filtrləmə tətbiq edir
        builder.HasIndex(x => x.IsDeleted)
            .HasFilter("[IsDeleted] = 0"); // SQL ifadəsində 0 false olaraq qəbul edilir
    }
}
