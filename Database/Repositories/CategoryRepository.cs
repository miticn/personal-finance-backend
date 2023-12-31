﻿using AutoMapper;
using Finance.Models;
using Microsoft.EntityFrameworkCore;
using Transaction.Database.Entities;
using Transaction.Models;

namespace Transaction.Database.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TransactionsDbContext _dbContext;
        private readonly IMapper _mapper;
        public CategoryRepository(TransactionsDbContext dbContext, IMapper mapper)
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

        public Task<List<CategoryEntity>> GetCategories(string? parentcode)
        {
            if(parentcode == null)
            {
                //get categories from db where parentcode is null
                return _dbContext.Categories.Where(p => p.ParentCode == "").ToListAsync();
            }
            //get categories from db
            var categories = _dbContext.Categories.Where(p => p.ParentCode == parentcode).ToListAsync();
            return categories;
        }
    }
}
