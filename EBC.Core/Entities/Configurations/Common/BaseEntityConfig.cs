using EBC.Core.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBC.Core.Entities.Configurations.Common;

public class BaseEntityConfig<T, Tid> : IEntityTypeConfiguration<T> where T : BaseEntity<Tid>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        //builder.Property(x => x.CreatedDate).ValueGeneratedOnAdd();
        //builder.Property(x => x.ModifiedDate).ValueGeneratedOnAddOrUpdate();

        //ancaq soft olaraq silinmemis deyerleri getirecek(legv etmek ucun isse repo icinde .IgnoreQueryFilters() methodunu ist elemek lazimdir)
        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}
