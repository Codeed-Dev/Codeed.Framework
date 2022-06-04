using Codeed.Framework.Commons;
using System;

namespace Codeed.Framework.Domain
{
    public interface IEntity : IId
    {
        DateTimeOffset CreatedDate { get; }
    }
}