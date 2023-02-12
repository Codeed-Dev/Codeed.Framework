using AutoMapper;
using Codeed.Framework.Data;
using Codeed.Framework.Domain;
using Codeed.Framework.Models;
using Codeed.Framework.Services;
using Microsoft.AspNetCore.Mvc;
using Codeed.Framework.Commons;
using Codeed.Framework.Domain.Validations;

namespace CodeedMeta.Core.Application.Services
{
    public static partial class CRUDHttpService
    {
        public static class CreateOrUpdate<TEntity> where TEntity : Entity, IAggregateRoot
        {
            public static class WithDto<TDtoRequest> where TDtoRequest : IDto
            {
                public abstract class Returning<TDtoResponse> : HttpService
                    .WithParameters<TDtoRequest>
                    .WithResponse<TDtoResponse>
                    where TDtoResponse : IDto
                {
                    private readonly IRepository<TEntity> _repository;
                    private readonly IMapper _mapper;
                    private readonly IEnumerable<ICreateValidation<TEntity>> _validations;

                    public Returning(IRepository<TEntity> repository, IMapper mapper, IEnumerable<ICreateValidation<TEntity>> validations)
                    {
                        _repository = repository;
                        _mapper = mapper;
                        _validations = validations;
                    }

                    [HttpPost]
                    public override async Task<TDtoResponse> ExecuteAsync(TDtoRequest request, CancellationToken cancellationToken)
                    {
                        var entity = await SaveEntity(request, cancellationToken);
                        var responseDto = _mapper.Map<TDtoResponse>(entity);
                        return responseDto;
                    }


                    [NonAction]
                    public async Task<TEntity> SaveEntity(TDtoRequest request, CancellationToken cancellationToken)
                    {
                        if (request == null)
                            throw new ArgumentNullException(nameof(request));

                        await ValidateRequest(request, cancellationToken);

                        var entity = await FindEntity(request, cancellationToken);
                        var isNew = entity == null;
                        if (isNew)
                        {
                            entity = await CreateEntity(request, cancellationToken);
                        }
                        else
                        {
                            await UpdateEntity(request, entity, cancellationToken);
                        }

                        await Validate(entity, cancellationToken);

                        Action<TEntity> modifyRepository = isNew ? _repository.Add : _repository.Update;
                        modifyRepository(entity);

                        await _repository.UnitOfWork.Commit(cancellationToken);
                        return entity;
                    }

                    protected virtual Task ValidateRequest(TDtoRequest request, CancellationToken cancellationToken)
                    {
                        request.Validate();
                        return Task.CompletedTask;
                    }

                    protected virtual async Task Validate(TEntity entity, CancellationToken cancellationToken)
                    {
                        foreach (var validation in _validations.OrderBy(v => v.Priority))
                        {
                            await validation.ValidateAsync(entity, cancellationToken);
                        }
                    }

                    protected abstract Task<TEntity> FindEntity(TDtoRequest request, CancellationToken cancellationToken);

                    protected abstract Task<TEntity> CreateEntity(TDtoRequest dtoRequest, CancellationToken cancellationToken);

                    protected abstract Task UpdateEntity(TDtoRequest request, TEntity entity, CancellationToken cancellationToken);

                }
            }

        }
    }
}
