using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codeed.Framework.Data;
using Codeed.Framework.Domain;
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
            public abstract class Returning<TDto> : HttpService
                .WithoutParameters
                .WithResponse<IQueryable<TDto>>
            {
                protected readonly IRepository<TEntity> Repository;
                protected readonly IMapper Mapper;

                protected Returning(IRepository<TEntity> repository, IMapper mapper)
                {
                    Repository = repository;
                    Mapper = mapper;
                }

                [ODataAttributeRouting]
                [EnableQuery(EnsureStableOrdering = false, PageSize = 100)]
                [HttpGet]
                public override Task<IQueryable<TDto>> ExecuteAsync(CancellationToken cancellationToken)
                {
                    var query = Repository.QueryAll();
                    query = ConfigureQuery(query);

                    return Task.FromResult(query.ProjectTo<TDto>(Mapper.ConfigurationProvider));
                }

                [HttpGet("count")]
                public virtual Task<int> Count([FromQuery] ODataQueryOptions<TDto> odata, CancellationToken cancellationToken)
                {
                    var query = Repository.QueryAll();
                    query = ConfigureQuery(query);
                    var odataQuery = odata.ApplyTo(query.ProjectTo<TDto>(Mapper.ConfigurationProvider)).Cast<TDto>();

                    return odataQuery.CountAsync(cancellationToken);
                }

                protected virtual IQueryable<TEntity> ConfigureQuery(IQueryable<TEntity> query)
                {
                    return query;
                }
            }

        }
    }
}
