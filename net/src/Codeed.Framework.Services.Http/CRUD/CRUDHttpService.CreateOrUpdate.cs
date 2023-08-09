using AutoMapper;
using Codeed.Framework.Data;
using Codeed.Framework.Domain;
using Codeed.Framework.Models;
using Microsoft.AspNetCore.Mvc;
using Codeed.Framework.Domain.Validations;

namespace Codeed.Framework.Services.CRUD
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
                    protected readonly IRepository<TEntity> Repository;
                    protected readonly IMapper Mapper;
                    private readonly IEnumerable<ICreateValidation<TEntity>> _createValidations;
                    private readonly IEnumerable<IUpdateValidation<TEntity>> _updateValidations;

                    protected Returning(
                        IRepository<TEntity> repository, 
                        IMapper mapper, 
                        IEnumerable<ICreateValidation<TEntity>> createValidations, 
                        IEnumerable<IUpdateValidation<TEntity>> updateValidations)
                    {
                        Repository = repository;
                        Mapper = mapper;
                        _createValidations = createValidations;
                        _updateValidations = updateValidations;
                    }

                    [HttpPost]
                    public override async Task<TDtoResponse> ExecuteAsync(TDtoRequest request, CancellationToken cancellationToken)
                    {
                        var entity = await SaveEntity(request, cancellationToken);
                        var responseDto = Mapper.Map<TDtoResponse>(entity);
                        return responseDto;
                    }


                    [NonAction]
                    public async Task<TEntity> SaveEntity(TDtoRequest request, CancellationToken cancellationToken)
                    {
                        if (request is null)
                            throw new ArgumentNullException(nameof(request));

                        await ValidateRequest(request, cancellationToken);

                        var entity = await FindEntity(request, cancellationToken);
                        if (entity is null)
                        {
                            entity = await Create(request, entity, cancellationToken);
                        }
                        else
                        {
                            await Update(request, entity, cancellationToken);
                        }

                        await Repository.UnitOfWork.Commit(cancellationToken);
                        return entity;
                    }

                    protected virtual async Task<TEntity> Create(TDtoRequest request, TEntity? entity, CancellationToken cancellationToken)
                    {
                        entity = await CreateEntity(request, cancellationToken);
                        await Validate(entity, _createValidations, cancellationToken);
                        Repository.Add(entity);
                        return entity;
                    }

                    protected virtual async Task Update(TDtoRequest request, TEntity entity, CancellationToken cancellationToken)
                    {
                        Repository.Update(entity);
                        await UpdateEntity(request, entity, cancellationToken);
                        await Validate(entity, _updateValidations, cancellationToken);
                    }

                    protected virtual Task ValidateRequest(TDtoRequest request, CancellationToken cancellationToken)
                    {
                        request.Validate();
                        return Task.CompletedTask;
                    }

                    protected virtual async Task Validate(TEntity entity, IEnumerable<IValidationOfT<TEntity>> validations, CancellationToken cancellationToken)
                    {
                        foreach (var validation in validations.OrderBy(v => v.Priority))
                        {
                            await validation.ValidateAsync(entity, cancellationToken);
                        }
                    }

                    protected abstract Task<TEntity?> FindEntity(TDtoRequest request, CancellationToken cancellationToken);

                    protected abstract Task<TEntity> CreateEntity(TDtoRequest dtoRequest, CancellationToken cancellationToken);

                    protected abstract Task UpdateEntity(TDtoRequest request, TEntity entity, CancellationToken cancellationToken);

                }
            }

        }
    }
}
