using AutoMapper;
using Sample.Application.Services.Customers.Models;
using Sample.Domain;

namespace Sample.Application.Services.Customers.Maps
{
    internal class CustomerMaps : Profile
    {
        public CustomerMaps()
        {
            CreateMap<Customer, CustomerDto>();
        }
    }
}
