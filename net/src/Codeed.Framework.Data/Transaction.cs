using System;
using System.Threading;
using System.Threading.Tasks;

namespace Codeed.Framework.Data
{
    public class Transaction : IDisposable
    {
        private IUnitOfWork _unitOfWork;

        internal Transaction(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> Commit(CancellationToken cancellationToken)
        {
            return _unitOfWork.Commit(cancellationToken);
        }

        public void Dispose()
        {
            _unitOfWork.EndTransaction(this);
        }
    }
}
