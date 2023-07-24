using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Finance.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Transaction.Database.Entities;
using Transaction.Database.Repositories;
using Transaction.Models;

namespace Transaction.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository productsRepository, IMapper mapper)
        {
            _transactionRepository = productsRepository;
            _mapper = mapper;
        }

        public async Task<Models.Transaction> GetTransaction(string id)
        {
            var trEntity = await _transactionRepository.Get(id);

            if (trEntity == null)
            {
                return null;
            }

            return _mapper.Map<Models.Transaction>(trEntity);
        }

        public async Task<List<Models.Transaction>> ParseCsv(string csvData)
        {
            using var reader = new StringReader(csvData);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null
            };

            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<CSVTransactionMap>();

            var transactions = csv.GetRecords<Models.Transaction>();
          
            return transactions.ToList();
        }

        public async Task ImportTransactions(List<Models.Transaction> transactions)
        {
            // Validation and Business Rules Here
            await _transactionRepository.AddTransactionsAsync(transactions);
            await _transactionRepository.SaveAsync();
        }

        public async Task<PagedSortedList<Models.Transaction>> GetTransactions(string transactionKind, DateTime? startDate, DateTime? endDate, string sortBy, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc)
        {
            var result = await _transactionRepository.List(transactionKind, startDate, endDate, sortBy, page, pageSize, sortOrder);

            return _mapper.Map<PagedSortedList<Models.Transaction>>(result);
        }
    }
}
