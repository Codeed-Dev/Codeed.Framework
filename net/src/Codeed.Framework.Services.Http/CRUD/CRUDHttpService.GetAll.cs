using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codeed.Framework.Data;
using Codeed.Framework.Domain;
using Codeed.Framework.Models;
using Codeed.Framework.Services.Http.CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Codeed.Framework.Services.CRUD
{
    public static partial class CRUDHttpService
    {
        public static class GetAll<TEntity> where TEntity : Entity, IAggregateRoot
        {
            public static class Returning<TDto> 
            {
                public abstract class WithTotals<TReturning> : HttpService
                    .WithParameters< ODataQueryOptions<TDto>>
                    .WithResponse<TReturning>
                    where TReturning : GetAllDto<TDto>
                {
                    protected readonly IRepository<TEntity> Repository;
                    protected readonly IMapper Mapper;

                    protected WithTotals(IRepository<TEntity> repository, IMapper mapper)
                    {
                        Repository = repository;
                        Mapper = mapper;
                    }

                    [ApiExplorerSettings(IgnoreApi = true)]
                    [HttpGet]
                    public override async Task<TReturning> ExecuteAsync([FromQuery] ODataQueryOptions<TDto> odata, CancellationToken cancellationToken)
                    {
                        IQueryable<TEntity> query = Repository.QueryAll();
                        query = ConfigureQuery(query);
                        var ignoreQueryOptions = AllowedQueryOptions.Skip | AllowedQueryOptions.Top;
                        var baseQuery = odata.ApplyTo(query.ProjectTo<TDto>(Mapper.ConfigurationProvider), ignoreQueryOptions).Cast<TDto>();
                        var queryDto = baseQuery.Skip(odata.Skip?.Value ?? 0).Take(odata.Top?.Value ?? 100);

                        var data = await queryDto.ToListAsync(cancellationToken) ?? Enumerable.Empty<TDto>();

                        var totals = await BuildTotals(baseQuery, cancellationToken);
                        totals.Data = data;
                        return totals;
                    }

                    protected virtual IQueryable<TEntity> ConfigureQuery(IQueryable<TEntity> query)
                    {
                        return query;
                    }

                    protected abstract Task<TReturning> BuildTotals(IQueryable<TDto> baseQuery, CancellationToken cancellationToken);
                }
            }
        }
    }
}
