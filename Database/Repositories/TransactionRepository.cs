using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Transaction.Database.Entities;
using Transaction.Models;

namespace Transaction.Database.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionsDbContext _dbContext;
        private readonly IMapper _mapper;
        public TransactionRepository(TransactionsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TransactionEntity> Get(string productCode)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(p => p.Id == productCode);
        }

        public async Task<PagedSortedList<TransactionEntity>> List(string transactionKind, DateTime? startDate, DateTime? endDate, string sortBy, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc)
        {
            var query = _dbContext.Transactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(transactionKind))
            {
                var kindValues = transactionKind.Split(',');
                var enumValues = new List<TransactionKindsEnum>();

                foreach (var kindValue in kindValues)
                {
                    if (Enum.TryParse<TransactionKindsEnum>(kindValue.Trim(), out var enumValue))
                    {
                        enumValues.Add(enumValue);
                    }
                }

                query = query.Where(t => enumValues.Contains(t.Kind));
            }

            if (startDate != null && (endDate != null))
                query = query.Where(t => t.Date >= startDate && t.Date <= endDate);
            else if (endDate != null)
                query = query.Where(t => t.Date <= endDate);
            else if (startDate != null)
                query = query.Where(t => t.Date >= startDate);

            switch (sortBy)
            {
                case "date":
                    query = sortOrder == SortOrder.Asc ? query.OrderBy(t => t.Date) : query.OrderByDescending(t => t.Date);
                    break;
                case "amount":
                    query = sortOrder == SortOrder.Asc ? query.OrderBy(t => t.Amount) : query.OrderByDescending(t => t.Amount);
                    break;
                case "id":
                    query = sortOrder == SortOrder.Asc ? query.OrderBy(t => t.Id) : query.OrderByDescending(t => t.Id);
                    break;
                default:
                    break;
            }

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();


            return new PagedSortedList<TransactionEntity>
            {
                PageSize = pageSize,
                Page = page,
                TotalCount = totalItems,
                SortBy = sortBy,
                SortOrder = sortOrder,
                Items = items
            };
        }

        public async Task AddTransactionsAsync(List<Models.Transaction> transactions)
        {

            var entities = _mapper.Map<List<TransactionEntity>>(transactions);
            await _dbContext.AddRangeAsync(entities);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
