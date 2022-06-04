﻿using Codeed.Framework.Domain;
using System;
using System.Linq;

namespace Influencer.Core.Data
{
    public interface IRepository<T> : IDisposable where T : Entity, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        IQueryable<T> QueryAll();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(IQueryable<T> entities);
    }
}