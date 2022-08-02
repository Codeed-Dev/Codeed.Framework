using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codeed.Framework.Services;
using Codeed.Framework.Services.Attributes;
using Codeed.Framework.Services.Http.Attributes;
using Microsoft.AspNetCore.Mvc;
using Sample.Application.Services.Customers.Models;
using Sample.Domain;
using Sample.Domain.Repositories;

namespace Sample.Application.Services.Customers
{
    [Route("api/customers")]
    [TenantAuthorize("enterprise")]
    [ApiExplorerSettings(GroupName = "Custom Group Name")]
    public class AddCustomers : HttpService
        .WithParameters<CustomerDto>
        .WithResponse<CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public AddCustomers(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a customer
        /// </summary>
        [HttpPost]
        public override async Task<CustomerDto> ExecuteAsync(CustomerDto customerDto, CancellationToken cancellationToken)
        {
            var customer = new Customer(customerDto.Code, customerDto.Description, customerDto.Identification);
            _customerRepository.Add(customer);
            await _customerRepository.UnitOfWork.Commit(cancellationToken);
            return _mapper.Map<CustomerDto>(customer);

        }
    }
}
