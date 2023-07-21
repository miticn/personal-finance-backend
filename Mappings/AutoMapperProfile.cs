using AutoMapper;
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

            //CreateMap<CreateProductCommand, TransactionEntity>()
            //   .ForMember(d => d.Code, opts => opts.MapFrom(s => s.ProductCode));
        }
    }
}
