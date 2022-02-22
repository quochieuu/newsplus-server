using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPlus.Data.Entities;

namespace NewsPlus.Data.Configurations
{
    public class SaveConfiguration : IEntityTypeConfiguration<Saved>
    {
        public void Configure(EntityTypeBuilder<Saved> builder)
        {
            builder.ToTable("Saves");
            builder.HasKey(x => x.Id);

        }
    }
}
