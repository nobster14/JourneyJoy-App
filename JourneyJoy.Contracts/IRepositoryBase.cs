using System.Linq.Expressions;

namespace JourneyJoy.Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();

        IQueryable<T> TakeSkip(int take, int skip);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        T? GetById(Guid id);
    }
}