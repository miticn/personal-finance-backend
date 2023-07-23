using AutoMapper;
using Finance.Models;
using Transaction.Database.Entities;
using Transaction.Models;

namespace Transaction.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TransactionEntity, Models.Transaction>();
            CreateMap<Models.Transaction, TransactionEntity>();
            CreateMap<PagedSortedList<TransactionEntity>, PagedSortedList<Models.Transaction>>();
            CreateMap<CategoryEntity, Category>();
            CreateMap<Category, CategoryEntity>();
            CreateMap<PagedSortedList<CategoryEntity>, PagedSortedList<Category>>();
            //CreateMap<CreateProductCommand, TransactionEntity>()
            //   .ForMember(d => d.Code, opts => opts.MapFrom(s => s.ProductCode));
        }
    }
}
