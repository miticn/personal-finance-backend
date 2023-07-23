using Microsoft.EntityFrameworkCore;
using Transaction.Database.Entities;
using System.Reflection;
using Finance.Models;

namespace Transaction.Database
{
    public class CategoriesDbContext : DbContext
    {
        public DbSet<CategoryEntity> Categories { get; set; }

        public CategoriesDbContext()
        {

        }

        public CategoriesDbContext(DbContextOptions<CategoriesDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
