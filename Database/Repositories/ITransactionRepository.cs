using Finance.Models;
using Microsoft.AspNetCore.Mvc;
using Transaction.Database.Entities;
using Transaction.Models;

namespace Transaction.Database.Repositories
{
    public interface ITransactionRepository
    {
        Task<PagedSortedList<Models.Transaction>> List(string transactionKind, DateTime? startDate, DateTime? endDate, string sortBy, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc);
        Task<TransactionEntity> Get(string id);
        Task AddTransactionsAsync(List<Models.Transaction> transactions);
        Task SaveAsync();
        Task<List<AnalyticsResult>> GetAnalytics(string? catcode, DateTime? startDate, DateTime? endDate, string? direction);
    }
}
