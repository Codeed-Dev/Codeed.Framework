using AutoMapper;
using AutoMapper.QueryableExtensions;
using Codeed.Framework.Data;
using Codeed.Framework.Domain;
using Codeed.Framework.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;

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
                private readonly IRepository<TEntity> _repository;
                private readonly IMapper _mapper;

                protected Returning(IRepository<TEntity> repository, IMapper mapper)
                {
                    _repository = repository;
                    _mapper = mapper;
                }

                [ODataAttributeRouting]
                [EnableQuery(PageSize = 100)]
                [HttpGet]
                public override Task<IQueryable<TDto>> ExecuteAsync(CancellationToken cancellationToken)
                {
                    return Task.FromResult(_repository.QueryAll().ProjectTo<TDto>(_mapper.ConfigurationProvider));
                }
            }

        }
    }
}
