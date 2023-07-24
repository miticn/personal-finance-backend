using Finance.Models;
using Microsoft.AspNetCore.Mvc;
using Transaction.Database.Entities;
using Transaction.Models;

namespace Transaction.Database.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryEntity> Get(string code);
        Task AddTransactionsAsync(List<Category> transactions);
        Task SaveAsync();
    }
}
