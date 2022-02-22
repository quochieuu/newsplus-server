using Microsoft.EntityFrameworkCore;
using NewsPlus.Data.Configurations;
using NewsPlus.Data.Entities;
using NewsPlus.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPlus.Data.EF
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure using Fluent API
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new RatingConfiguration());
            modelBuilder.ApplyConfiguration(new SaveConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriberConfiguration());

            modelBuilder.ApplyConfiguration(new ConfigConfiguration());

            modelBuilder.ApplyConfiguration(new SysAppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new SysAppUserConfiguration());


            //Data seeding
            modelBuilder.Seed();
            //base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Saved> Saved { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<SysAppUser> SysAppUsers { get; set; }
        public DbSet<SysAppRole> SysAppRoles { get; set; }
    }
}
