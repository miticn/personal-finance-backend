using Microsoft.AspNetCore.Mvc;
using Transaction.Database.Entities;
using Transaction.Models;

namespace Transaction.Database.Repositories
{
    public interface ITransactionRepository
    {
        Task<PagedSortedList<TransactionEntity>> List(string transactionKind, DateTime? startDate, DateTime? endDate, string sortBy, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc);
        Task<TransactionEntity> Get(string productCode);
        Task AddTransactionsAsync(List<Models.Transaction> transactions);
        Task SaveAsync();
    }
}
