using AutoMapper;
using Codeed.Framework.Data;
using Codeed.Framework.Domain;
using Codeed.Framework.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeedMeta.Core.Application.Services
{
    public static partial class CRUDHttpService
    {
        public static class GetById<TEntity> where TEntity : Entity, IAggregateRoot
        {
            public abstract class Returning<TDto> : HttpService
                .WithParameters<Guid>
                .WithResponse<TDto>
            {
                private readonly IRepository<TEntity> _repository;
                private readonly IMapper _mapper;

                public Returning(IRepository<TEntity> repository, IMapper mapper)
                {
                    _repository = repository;
                    _mapper = mapper;
                }

                [HttpGet("{id}")]
                public override async Task<TDto> ExecuteAsync(Guid id, CancellationToken cancellationToken)
                {
                    var query = _repository.QueryById(id);
                    query = ConfigureQuery(query);

                    var entity = await query.FirstOrDefaultAsync(cancellationToken);
                    return _mapper.Map<TDto>(entity);
                }

                protected virtual IQueryable<TEntity> ConfigureQuery(IQueryable<TEntity> query)
                {
                    return query;
                }
            }
        }
    }
}
