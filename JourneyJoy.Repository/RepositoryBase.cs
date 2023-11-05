using JourneyJoy.Contracts;
using JourneyJoy.Model.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JourneyJoy.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        #region Properties

        public DatabaseContext DatabaseContext { get; set; }

        #endregion

        #region Constructors

        public RepositoryBase(DatabaseContext context) => DatabaseContext = context;

        #endregion
        public virtual void Create(T entity)
        {
            DatabaseContext.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            DatabaseContext.Set<T>().Remove(entity);
        }

        public virtual IQueryable<T> FindAll()
        {
            return DatabaseContext.Set<T>().AsNoTracking();
        }

        public virtual IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return DatabaseContext.Set<T>().Where(expression).AsNoTracking();
        }

        public virtual void Update(T entity)
        {
            var context = DatabaseContext.Set<T>();
            var entry = context.Attach(entity);
            var entityType = context.Entry(entity).Metadata;
            foreach (var property in entityType.GetProperties())
            {
                if (property.IsPrimaryKey())
                    continue;

                var currentValue = property.PropertyInfo?.GetValue(entity);
                if (currentValue != null)
                {
                    entry.Property(property.Name).IsModified = true;
                }
            }
        }

        public virtual T? GetById(Guid id)
        {
            return DatabaseContext.Set<T>().Find(id);
        }

        public virtual IQueryable<T> TakeSkip(int take, int skip)
        {
            return DatabaseContext.Set<T>().Skip(skip).Take(take);
        }
    }
}