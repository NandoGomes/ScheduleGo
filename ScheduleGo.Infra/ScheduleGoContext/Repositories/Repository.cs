using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleGo.Domain.ScheduleGoContext.Repositories;
using ScheduleGo.Infra.ScheduleGoContext.DataContexts;
using ScheduleGo.Shared.ScheduleGoContext.Entities;

namespace ScheduleGo.Infra.ScheduleGoContext.Repositories
{
	public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
	{
		protected readonly ScheduleGoDataContext context;
		protected readonly ILogger<Repository<TEntity>> logger;

		public Repository(ScheduleGoDataContext context,
					ILogger<Repository<TEntity>> logger)
		{
			this.context = context;
			this.logger = logger;
		}

		public TEntity Add(TEntity entity)
		{
			try
			{
				context.Set<TEntity>().Add(entity);
				context.SaveChanges();
			}
			catch (Exception exception)
			{
				logger.LogError(exception.ToString());
			}

			return entity;
		}

		public IEnumerable<TEntity> GetAll()
		{
			IEnumerable<TEntity> result = null;

			try
			{
				result = context.Set<TEntity>();
			}
			catch (Exception exception)
			{
				logger.LogError(exception.ToString());
			}

			return result;
		}

		public IEnumerable<TEntity> GetAll(int page, int pageSize)
		{
			IEnumerable<TEntity> result = null;

			try
			{
				result = context.Set<TEntity>().Skip((page - 1) * pageSize).Take(pageSize);
			}
			catch (Exception exception)
			{
				logger.LogError(exception.ToString());
			}

			return result;
		}

		public TEntity Get(params object[] id)
		{
			TEntity entity = null;

			try
			{
				entity = context.Set<TEntity>().Find(id);
			}
			catch (Exception exception)
			{
				logger.LogError(exception.ToString());
			}

			return entity;
		}

		public void Remove(TEntity entity)
		{
			try
			{
				context.Set<TEntity>().Remove(entity);
				context.SaveChanges();
			}
			catch (Exception exception)
			{
				logger.LogError(exception.ToString());
			}
		}

		public void Update(TEntity entity)
		{
			try
			{
				context.Entry(entity).State = EntityState.Modified;
				context.SaveChanges();
			}
			catch (Exception exception)
			{
				logger.LogError(exception.ToString());
			}
		}

		public void Dispose() => context.Dispose();

		public bool Exists(params object[] id)
		{
			bool result = false;

			try
			{
				result = context.Set<TEntity>().Find(id) != null;
			}
			catch (Exception exception)
			{
				logger.LogError(exception.ToString());
			}

			return result;
		}

		public void Save()
		{
			try
			{
				context.SaveChanges();
			}
			catch (Exception exception)
			{
				logger.LogError(exception.ToString());
			}
		}

		public IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> expression)
		{
			IEnumerable<TEntity> result = null;

			try
			{
				result = context.Set<TEntity>().Where(expression);
			}
			catch (Exception exception)
			{
				logger.LogError(exception.ToString());
			}

			return result;
		}

		public IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> expression, int page, int pageSize)
		{
			IEnumerable<TEntity> result = null;

			try
			{
				result = context.Set<TEntity>().Where(expression).Skip((page - 1) * pageSize).Take(page);
			}
			catch (Exception exception)
			{
				logger.LogError(exception.ToString());
			}

			return result;
		}
	}
}