using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Domain.ScheduleGoContext.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        TEntity Add(TEntity entity);
        int Count();
        bool Exists(params object[] id);
        TEntity Get(params object[] id);
        IEnumerable<TEntity> GetAll(int page, int pageSize);
        IEnumerable<TEntity> GetAll();
        void Remove(TEntity entity);
        IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> searchExpression, int page, int pageSize);
        void Update(TEntity entity);
    }
}