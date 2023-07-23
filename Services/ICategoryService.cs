using Microsoft.AspNetCore.Mvc;
using Transaction.Database.Entities;
using Transaction.Models;

namespace Finance.Services
{
    public interface ICategoryService
    {
        Task<PagedSortedList<Models.Category>> GetCategories(string parentId);
        Task<List<Models.Category>> ParseCsv(string csvData);
        Task ImportCategories(List<Models.Category> categories);
    }
}
