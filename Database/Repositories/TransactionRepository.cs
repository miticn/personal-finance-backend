using AutoMapper;
using Finance.Models;
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

        public async Task<TransactionEntity> Get(string id)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(p => p.Id == id);
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

        public async Task<List<AnalyticsResult>> GetAnalytics(string? catcode, DateTime? startDate, DateTime? endDate, string? direction)
        {
            var query = _dbContext.Transactions.AsQueryable();
            var catWithParents = await _dbContext.Categories.Where(e => e.ParentCode != null && e.ParentCode!= "").ToListAsync();

            if (startDate != null && (endDate != null))
                query = query.Where(t => t.Date >= startDate && t.Date <= endDate);
            else if (endDate != null)
                query = query.Where(t => t.Date <= endDate);
            else if (startDate != null)
                query = query.Where(t => t.Date >= startDate);

            if (direction != null && (direction == "d" || direction == "c"))
            {
                query = query.Where(t => t.Direction == (DirectionsEnum)Enum.Parse(typeof(DirectionsEnum), direction));
            }

            var toKeep = new List<string>();
            if(!string.IsNullOrWhiteSpace(catcode))
            {
                var catValues = catcode.Split(',');
                var enumValues = new List<string>();

                foreach (var catSearchValue in catValues)
                {
                    var val = catSearchValue.Trim();
                    enumValues.Add(val);
                    toKeep.Add(val);
                    //find all children
                    var children = catWithParents.Where(c => c.ParentCode == val).Select(c => c.Code).ToList();
                    enumValues.AddRange(children);
                }
                query = query.Where(t => enumValues.Contains(t.Catcode));
            }

            var query2 = await query.GroupBy(t => t.Catcode)
                .Select(g => new AnalyticsResult
                {
                    catcode = g.Key,
                    amount = g.Sum(t => t.Amount),
                    count = g.Count()
                }).ToListAsync();

            

            var joined = query2.Join(catWithParents, t => t.catcode, c => c.Code, (t, c) => new AnalyticsResult
            {
                catcode = c.ParentCode,
                amount = t.amount,
                count = t.count
            }).Union(query2);

            //sum count and sum for same category
            joined = joined.GroupBy(t => t.catcode)
                .Select(g => new AnalyticsResult
                {
                    catcode = g.Key,
                    amount = g.Sum(t => t.amount),
                    count = g.Sum(t => t.count)
                });

            //keep only toKeep
            if(toKeep.Count > 0)
            {
                joined = joined.Where(t => toKeep.Contains(t.catcode));
            }
            return joined.ToList();

        }

    }
}
