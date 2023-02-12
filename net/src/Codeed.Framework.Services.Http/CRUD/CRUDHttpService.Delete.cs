using Codeed.Framework.Commons.Exceptions;
using Codeed.Framework.Data;
using Codeed.Framework.Domain;
using Codeed.Framework.Domain.Validations;
using Codeed.Framework.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeedMeta.Core.Application.Services
{
    public static partial class CRUDHttpService
    {
        public abstract class Delete<TEntity> : HttpService
            .WithParameters<Guid>
            .WithoutResponse
            where TEntity : Entity, IAggregateRoot
        {
            private readonly IRepository<TEntity> _repository;
            private readonly ILogger<Delete<TEntity>> _logger;
            private readonly IEnumerable<IDeleteValidation<TEntity>> _validations;

            protected Delete(IRepository<TEntity> repository, ILogger<Delete<TEntity>> logger, IEnumerable<IDeleteValidation<TEntity>> validations)
            {
                _repository = repository;
                _logger = logger;
                _validations = validations;
            }

            [HttpDelete("{id}")]
            public override async Task ExecuteAsync(Guid id, CancellationToken cancellationToken)
            {
                var entity = await FindEntity(id, cancellationToken);
                if (entity == null)
                    return;

                await Validate(entity, cancellationToken);

                if (entity is IDeletableEntity deletableEntity)
                {
                    deletableEntity.OnDelete();
                }

                await DeleteEntity(entity, cancellationToken);

                try
                {
                    await _repository.UnitOfWork.Commit(cancellationToken);
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Error on delete entity {0}", entity.GetType().Name);
                    throw new ServiceException("Deleted record is being used", "delete/record-is-being-used", new { id });
                }
            }

            protected virtual Task DeleteEntity(TEntity entity, CancellationToken cancellationToken)
            {
                _repository.Delete(entity);
                return Task.CompletedTask;
            }

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
