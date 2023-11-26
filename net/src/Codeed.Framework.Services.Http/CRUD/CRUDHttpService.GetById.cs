using AutoMapper;
using Codeed.Framework.Data;
using Codeed.Framework.Domain;
using Codeed.Framework.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Codeed.Framework.Services.CRUD
{
    public static partial class CRUDHttpService
    {
        public static class GetById<TEntity> where TEntity : Entity, IAggregateRoot
        {
            public abstract class Returning<TDto> : HttpService
                .WithParameters<Guid>
                .WithResponse<TDto>
            {
                protected readonly IRepository<TEntity> Repository;
                protected readonly IMapper Mapper;

                protected Returning(IRepository<TEntity> repository, IMapper mapper)
                {
                    Repository = repository;
                    Mapper = mapper;
                }

                [HttpGet("{id}")]
                public override async Task<TDto> ExecuteAsync(Guid id, CancellationToken cancellationToken)
                {
                    var entity = await Repository.GetById(id, cancellationToken);
                    return Mapper.Map<TDto>(entity);
                }
            }
        }
    }
}
