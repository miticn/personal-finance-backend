using CsvHelper.Configuration;
using CsvHelper;
using Finance.Mappings;
using Finance.Models;
using System.Globalization;
using Transaction.Models;
using AutoMapper;
using Transaction.Database.Repositories;

namespace Finance.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<PagedSortedList<Category>> GetCategories(string parentId)
        {
            throw new NotImplementedException();
        }

        public async Task ImportCategories(List<Category> categories)
        {
            await _categoryRepository.AddTransactionsAsync(categories);
            await _categoryRepository.SaveAsync();

        }

        public async Task<List<Category>> ParseCsv(string csvData)
        {
            using var reader = new StringReader(csvData);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null
            };

            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<CSVCategoryMap>();

            var categories = csv.GetRecords<Category>();

            return categories.ToList();
        }
    }
}
