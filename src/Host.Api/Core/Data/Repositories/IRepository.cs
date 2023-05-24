﻿using Host.Api.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Host.Api.Core.Data.Repositories
{
    public interface IRepository<T> : IDisposable where T : AggregateRoot
    {
        public DbSet<T> Set { get; }

        Task<T?> FindAsync(long id);

        Task<T?> FirstOrDefautAsync(Expression<Func<T, bool>> predicate);

        Task<T?> SingleOrDefautAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);

        void Update(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}