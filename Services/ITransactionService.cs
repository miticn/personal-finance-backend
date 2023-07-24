using Microsoft.AspNetCore.Mvc;
using Transaction.Models;

namespace Transaction.Services
{
    public interface ITransactionService
    {
        Task<PagedSortedList<Models.Transaction>> GetTransactions(string transactionKind,
            DateTime? startDate,
            DateTime? endDate,
            string sortBy,
            int page=1,
            int pageSize=10,
            SortOrder sortOrder = SortOrder.Asc);
        Task<Models.Transaction> GetTransaction(string id);
        Task<List<Models.Transaction>> ParseCsv(string csvData);
        Task ImportTransactions(List<Models.Transaction> transactions);
    }
}
