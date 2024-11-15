using EBC.Core.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EBC.Core.Entities.Configurations.Common;

public class BaseEntityConfig<T, Tid> : IEntityTypeConfiguration<T> where T : BaseEntity<Tid>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        //builder.Property(x => x.CreatedDate).ValueGeneratedOnAdd();
        //builder.Property(x => x.ModifiedDate).ValueGeneratedOnAddOrUpdate();

        // Global Filter query əlavəsi üçün. ancaq soft olaraq silinmemis deyerleri getirecek(legv etmek ucun isse repo icinde .IgnoreQueryFilters() methodunu ist elemek lazimdir)
        builder.HasQueryFilter(x => !x.IsDeleted);

        // IsDeleted=false olan qeydlər üzrə indeks yaradır və filtrləmə tətbiq edir
        builder.HasIndex(x => x.IsDeleted)
            .HasFilter("[IsDeleted] = 0"); // SQL ifadəsində 0 false olaraq qəbul edilir
    }
}
