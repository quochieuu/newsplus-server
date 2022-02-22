using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsPlus.Data.Entities;

namespace NewsPlus.Data.Configurations
{
    public class SysAppRoleConfiguration : IEntityTypeConfiguration<SysAppRole>
    {
        public void Configure(EntityTypeBuilder<SysAppRole> builder)
        {
            builder.ToTable("SysAppRoles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
        }
    }
}
