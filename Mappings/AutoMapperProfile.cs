using AutoMapper;
using Finance.Commands;
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
            //CreateMap<TransactionSplitEntity, TransactionSplit>();
            //CreateMap<TransactionSplit, TransactionSplitEntity>();
            //CreateMap<List<TransactionSplitEntity>, List<TransactionSplit>>();

            //CreateMap<CreateProductCommand, TransactionEntity>()
            //   .ForMember(d => d.Code, opts => opts.MapFrom(s => s.ProductCode));
        }
    }
}
