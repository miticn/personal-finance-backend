using Finance.Models;
using Transaction.Database.Entities;
using Transaction.Models;

namespace Transaction.Database.Repositories
{
    public interface ITransactionSplitRepository
    {
            Task<List<TransactionSplitEntity>> GetSplits(string id);
            Task DeleteSplits(string id);
            Task AddTransactionsAsync(List<TransactionSplit> splits);
            Task SaveAsync();
    }
}
