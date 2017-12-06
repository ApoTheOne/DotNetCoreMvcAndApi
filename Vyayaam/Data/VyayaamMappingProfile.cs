using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vyayaam.Data.Entities;
using Vyayaam.ViewModels;

namespace Vyayaam.Data
{
    public class VyayaamMappingProfile : Profile
    {
        public VyayaamMappingProfile()
        {
            CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.OrderId, x => x.MapFrom(o => o.Id))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>()
                .ReverseMap();
        }
    }
}
