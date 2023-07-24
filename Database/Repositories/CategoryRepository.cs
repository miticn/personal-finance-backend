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

        public async Task<CategoryEntity> Get(string code)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task AddTransactionsAsync(List<Category> categories)
        {

            var entities = _mapper.Map<List<CategoryEntity>>(categories);

            foreach (var entity in entities)
            {
                var existingEntity = await _dbContext.Categories.FindAsync(entity.Code);

                if (existingEntity != null)
                {
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    await _dbContext.Categories.AddAsync(entity);
                }
            }

        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
