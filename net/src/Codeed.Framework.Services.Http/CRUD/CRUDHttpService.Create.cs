using AutoMapper;
using Codeed.Framework.Data;
using Codeed.Framework.Domain;
using Codeed.Framework.Domain.Validations;
using Codeed.Framework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Codeed.Framework.Services.CRUD
{
    public static partial class CRUDHttpService
    {
        public static class Create<TEntity> where TEntity : Entity, IAggregateRoot
        {
            public static class WithDto<TDtoRequest> where TDtoRequest : IDto
            {
                public abstract class Returning<TDtoResponse> : HttpService
                    .WithParameters<TDtoRequest>
                    .WithResponse<TDtoResponse>
                {
                    protected readonly IRepository<TEntity> Repository;
                    protected readonly IMapper Mapper;
                    private readonly IEnumerable<ICreateValidation<TEntity>> _validations;

                    protected Returning(IRepository<TEntity> repository, IMapper mapper, IEnumerable<ICreateValidation<TEntity>> validations)
                    {
                        Repository = repository;
                        Mapper = mapper;
                        _validations = validations;
                    }

                    [HttpPost]
                    public override async Task<TDtoResponse> ExecuteAsync(TDtoRequest request, CancellationToken cancellationToken)
                    {
                        if (request == null)
                        {
                            throw new ArgumentNullException(nameof(request));
                        }

                        request.Validate();
                        var entity = await CreateEntity(request, cancellationToken);
                        await Validate(entity, cancellationToken);
                        Repository.Add(entity);
                        await AfterAddEntity(request, entity, cancellationToken);

                        await Repository.UnitOfWork.Commit(cancellationToken);
                        var responseDto = Mapper.Map<TDtoResponse>(entity);
                        return responseDto;

                    }

                    protected virtual async Task Validate(TEntity entity, CancellationToken cancellationToken)
                    {
                        foreach (var validation in _validations.OrderBy(v => v.Priority))
                        {
                            await validation.ValidateAsync(entity, cancellationToken);
                        }
                    }

                    protected abstract Task<TEntity> CreateEntity(TDtoRequest request, CancellationToken cancellationToken);

                    protected virtual Task AfterAddEntity(TDtoRequest request, TEntity entity, CancellationToken cancellationToken)
                    {
                        return Task.CompletedTask;
                    }
                }
            }

        }
    }
}
