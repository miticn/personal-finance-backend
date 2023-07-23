using AutoMapper;
using Finance.Models;
using Microsoft.EntityFrameworkCore;
using Transaction.Database.Entities;
using Transaction.Models;

namespace Transaction.Database.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoriesDbContext _dbContext;
        private readonly IMapper _mapper;
        public CategoryRepository(CategoriesDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CategoryEntity> Get(string productCode)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(p => p.ParentCode == productCode);
        }

        public async Task AddTransactionsAsync(List<Category> categories)
        {

            var entities = _mapper.Map<List<CategoryEntity>>(categories);
            await _dbContext.AddRangeAsync(entities);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
