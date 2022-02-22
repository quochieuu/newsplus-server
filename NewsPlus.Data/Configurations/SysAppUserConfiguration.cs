using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPlus.Data.Entities;

namespace NewsPlus.Data.Configurations
{
    public class SysAppUserConfiguration : IEntityTypeConfiguration<SysAppUser>
    {
        public void Configure(EntityTypeBuilder<SysAppUser> builder)
        {
            builder.ToTable("SysAppUsers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Username).IsRequired();
        }
    }
}
