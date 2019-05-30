using Marketplace.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace Marketplace.Infrastructure
{
    public class ClassifiedAdDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public ClassifiedAdDbContext(
            DbContextOptions<ClassifiedAdDbContext> options,
            ILoggerFactory loggerFactory)
        : base(options) => _loggerFactory = loggerFactory;

        public DbSet<ClassifiedAd> ClassifiedAds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
            => modelBuilder.ApplyConfiguration(new ClassifiedAdEntityTypeConfiguration());
    }

    public class ClassifiedAdEntityTypeConfiguration : IEntityTypeConfiguration<ClassifiedAd>
    {
        public void Configure(EntityTypeBuilder<ClassifiedAd> builder) 
            => builder.HasKey(x => x.ClassifiedAdId);
    }

    public static class AppBuilderDatabaseExtensions
    {
        public static void EnsureDatabase(this IApplicationBuilder app)
        {
            var context = (ClassifiedAdDbContext)app.ApplicationServices
                .GetService(typeof(ClassifiedAdDbContext));

            if (!context.Database.EnsureCreated())
                context.Database.Migrate();
        }
    }
}