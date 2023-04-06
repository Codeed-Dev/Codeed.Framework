using AutoMapper;
using Codeed.Framework.Commons.Exceptions;
using Codeed.Framework.Data;
using Codeed.Framework.Domain;
using Codeed.Framework.Domain.Validations;
using Codeed.Framework.Models;
using Codeed.Framework.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Codeed.Framework.Services.CRUD
{
    public static partial class CRUDHttpService
    {
        public static class Update<TEntity> where TEntity : Entity, IAggregateRoot
        {
            public static class WithDto<TDtoRequest> where TDtoRequest : IDto
            {
                public abstract class Returning<TDtoResponse> : HttpService
                    .WithParameters<Guid, TDtoRequest>
                    .WithResponse<TDtoResponse>
                {
                    protected readonly IRepository<TEntity> Repository;
                    protected readonly IMapper Mapper;
                    private readonly IEnumerable<IUpdateValidation<TEntity>> _validations;

                    protected Returning(IRepository<TEntity> repository, IMapper mapper, IEnumerable<IUpdateValidation<TEntity>> validations)
                    {
                        Repository = repository;
                        Mapper = mapper;
                        _validations = validations;
                    }

                    [HttpPut("{id}")]
                    public override async Task<TDtoResponse> ExecuteAsync(Guid id, TDtoRequest request, CancellationToken cancellationToken)
                    {
                        if (request == null)
                        {
                            throw new ArgumentNullException(nameof(request));
                        }

                        request.Validate();
                        var entity = await FindEntity(id, cancellationToken);

                        if (entity is null)
                        {
                            throw new ServiceNotFoundException("The record was not found");
                        }

                        await UpdateEntity(request, entity, cancellationToken);
                        await Validate(entity, cancellationToken);
                        Repository.Update(entity);

                        await Repository.UnitOfWork.Commit(cancellationToken);
                        var responseDto = Mapper.Map<TDtoResponse>(entity);
                        return responseDto;

                    }

                    protected abstract Task UpdateEntity(TDtoRequest request, TEntity entity, CancellationToken cancellationToken);

                    protected virtual async Task Validate(TEntity entity, CancellationToken cancellationToken)
                    {
                        foreach (var validation in _validations.OrderBy(v => v.Priority))
                        {
                            await validation.ValidateAsync(entity, cancellationToken);
                        }
                    }

                    protected virtual async Task<TEntity?> FindEntity(Guid id, CancellationToken cancellationToken)
                    {
                        var entity = await Repository.QueryById(id)
                                                     .FirstOrDefaultAsync(cancellationToken);

                        return entity;
                    }
                }
            }

        }
    }
}
