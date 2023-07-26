namespace Transaction.Database.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Transaction.Database;
    using Transaction.Database.Entities;

    public class TransactionSplitRepository : ITransactionSplitRepository
    {
        private readonly TransactionsDbContext _dbContext;

        public TransactionSplitRepository(TransactionsDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<TransactionSplitEntity>> GetSplits(string id)
        {
            // Implement logic to retrieve splits based on the given transaction ID
            return await _dbContext.TransactionSplits
                .Where(split => split.TransactionId == id)
                .ToListAsync();
        }

        public async Task DeleteSplits(string id)
        {
            // Implement logic to delete splits based on the given transaction ID
            var splitsToDelete = await _dbContext.TransactionSplits
                .Where(split => split.TransactionId == id)
                .ToListAsync();

            _dbContext.TransactionSplits.RemoveRange(splitsToDelete);
        }

        public async Task AddTransactionsAsync(List<TransactionSplit> splits)
        {
            // Convert TransactionSplit to TransactionSplitEntity and add to the database
            foreach (var split in splits)
            {
                var splitEntity = new TransactionSplitEntity
                {
                    TransactionId = split.TransactionId,
                    catcode = split.catcode,
                    amount = split.amount
                };

                _dbContext.TransactionSplits.Add(splitEntity);
            }
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
