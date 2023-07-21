using Microsoft.EntityFrameworkCore;
using Transaction.Database.Entities;
using System.Reflection;

namespace Transaction.Database
{
    public class TransactionsDbContext: DbContext
    {
        public DbSet<TransactionEntity> Transactions { get; set; }

        public TransactionsDbContext()
        {

        }

        public TransactionsDbContext(DbContextOptions options): base(options)
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
