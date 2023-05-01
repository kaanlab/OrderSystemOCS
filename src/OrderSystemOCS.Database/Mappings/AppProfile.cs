using AutoMapper;
using OrderSystemOCS.Database.Models;
using OrderSystemOCS.Domain;

namespace OrderSystemOCS.Database.Mappings
{
    internal sealed class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<Order, OrderDb>()
                .ForMember(dest => dest.Lines, opt => opt.Ignore());

            CreateMap<OrderDb, Order>();

            CreateMap<ProductDb, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Id));

            CreateMap<LineDb, Line>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(x => x.Product))
                .ForMember(dest => dest.Qty, opt => opt.MapFrom(x => x.Qty));
        }
    }
}
