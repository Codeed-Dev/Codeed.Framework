using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codeed.Framework.Data;
using Codeed.Framework.Services;
using Codeed.Framework.Services.Attributes;
using Codeed.Framework.Services.CRUD;
using Codeed.Framework.Services.Http.CRUD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Application.Services.Customers.Models;
using Sample.Domain;
using Sample.Domain.Repositories;

namespace Sample.Application.Services.Customers
{
    [Route("api/customers")]
    public class GetAllCustomers : CRUDHttpService.GetAll<Customer>
        .Returning<CustomerDto>
        .WithTotals<GetAllDto<CustomerDto>>
    {
        public GetAllCustomers(IRepository<Customer> repository, IMapper mapper) : base(repository, mapper)
        {
        }


        protected override Task<GetAllDto<CustomerDto>> BuildTotals(IQueryable<CustomerDto> baseQuery, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
