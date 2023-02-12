using Codeed.Framework.Data;
using Microsoft.AspNetCore.Mvc;

namespace Codeed.Framework.Services.CRUD
{
    public static partial class CRUDHttpService
    {
        public static class Bulk
        {
            public static class WithDto<TDtoRequest>
            {
                public abstract class Returning<TDtoResponse> : HttpService
                    .WithParameters<IEnumerable<TDtoRequest>>
                    .WithResponse<IEnumerable<TDtoResponse>>
                {

                    [HttpPost]
                    public override async Task<IEnumerable<TDtoResponse>> ExecuteAsync(IEnumerable<TDtoRequest> requests, CancellationToken cancellationToken)
                    {
                        var unitOfWork = GetUnitOfWork();
                        var responses = new List<TDtoResponse>();

                        using (var transaction = unitOfWork.BeginTransaction())
                        {
                            foreach (var request in requests)
                            {
                                var dto = await Create(request, cancellationToken);
                                responses.Add(dto);
                            }

                            await transaction.Commit(cancellationToken);
                        }

                        return responses;
                    }

                    protected abstract Task<TDtoResponse> Create(TDtoRequest request, CancellationToken cancellationToken);

                    protected abstract IUnitOfWork GetUnitOfWork();
                }
            }

        }
    }
}
