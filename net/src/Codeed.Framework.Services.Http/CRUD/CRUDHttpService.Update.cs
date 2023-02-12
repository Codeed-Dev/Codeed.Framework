using AutoMapper;
using Codeed.Framework.Commons.Exceptions;
using Codeed.Framework.Data;
using Codeed.Framework.Domain;
using Codeed.Framework.Domain.Validations;
using Codeed.Framework.Models;
using Codeed.Framework.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeedMeta.Core.Application.Services
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
                    private readonly IRepository<TEntity> _repository;
                    private readonly IMapper _mapper;
                    private readonly IEnumerable<IUpdateValidation<TEntity>> _validations;

                    protected Returning(IRepository<TEntity> repository, IMapper mapper, IEnumerable<IUpdateValidation<TEntity>> validations)
                    {
                        _repository = repository;
                        _mapper = mapper;
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

                        if (entity == null)
                        {
                            throw new ServiceNotFoundException("The record was not found");
                        }

                        await UpdateEntity(request, entity, cancellationToken);
                        await Validate(entity, cancellationToken);
                        _repository.Update(entity);

                        await _repository.UnitOfWork.Commit(cancellationToken);
                        var responseDto = _mapper.Map<TDtoResponse>(entity);
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

                    protected virtual async Task<TEntity> FindEntity(Guid id, CancellationToken cancellationToken)
                    {
                        var entity = await _repository.QueryById(id)
                                                      .FirstOrDefaultAsync(cancellationToken);

                        return entity;
                    }
                }
            }

        }
    }
}
