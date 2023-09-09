using MicroserviceProject.Services.Catalog.Catalog.API.Infrastructure.EntityConfigurations;
using MicroserviceProject.Services.Catalog.Catalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace MicroserviceProject.Services.Catalog.Catalog.API.Infrastructure
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }
        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
            builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
        }
    }


    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
    {
        public CatalogContext CreateDbContext(string[] args)
        {
            // var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

            var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
                .UseSqlServer("Server=sqldata,5433;Initial Catalog=MicroserviceProject.Services.CatalogDb;User Id=sa;Password=MyStrongPassword123!;TrustServerCertificate=true");


            return new CatalogContext(optionsBuilder.Options);
        }
    }
}
