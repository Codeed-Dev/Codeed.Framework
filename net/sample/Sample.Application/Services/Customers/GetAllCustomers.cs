using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codeed.Framework.Services;
using Codeed.Framework.Services.Attributes;
using Microsoft.AspNetCore.Mvc;
using Sample.Application.Services.Customers.Models;
using Sample.Domain.Repositories;

namespace Sample.Application.Services.Customers
{
    [Route("api/customers")]
    public class GetAllCustomers : HttpService
        .WithoutParameters
        .WithResponse<IQueryable<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetAllCustomers(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        [HttpGet]
        public override Task<IQueryable<CustomerDto>> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_customerRepository.QueryAll().ProjectTo<CustomerDto>(_mapper.ConfigurationProvider));
        }
    }
}
