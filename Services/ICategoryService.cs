using Finance.Models;
using Microsoft.AspNetCore.Mvc;
using Transaction.Database.Entities;
using Transaction.Models;

namespace Finance.Services
{
    public interface ICategoryService
    {
        Task<List<Models.Category>> GetCategories(string parentId);
        Task<List<Models.Category>> ParseCsv(string csvData);
        Task ImportCategories(List<Models.Category> categories);
        Task<Category> GetCategory(string id);

        
    }
}
