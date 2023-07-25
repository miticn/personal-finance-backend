using Microsoft.EntityFrameworkCore;
using Transaction.Database.Entities;
using System.Reflection;
using Finance.Models;

namespace Transaction.Database
{
    public class TransactionsDbContext: DbContext
    {
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public TransactionsDbContext()
        {

        }

        public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options): base(options)
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
