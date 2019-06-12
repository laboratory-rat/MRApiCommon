using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MRApiCommon.Infrastructure.Interface
{
    /// <summary>
    /// Common interface for MRRepository instance
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IMRRepository<TEntity, TKey>
        where TEntity : class, IMREntity<TKey>, new()
    {
        #region create

        Task<TEntity> Insert(TEntity entity);
        Task<IEnumerable<TEntity>> Insert(IEnumerable<TEntity> entities);

        #endregion

        #region get

        Task<TEntity> Get(TKey id);
        Task<IEnumerable<TEntity>> Get(IEnumerable<TKey> ids);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> search);
        Task<TEntity> Get<F>(Expression<Func<TEntity, F>> field, F value);

        Task<IEnumerable<TEntity>> GetSorted(Expression<Func<TEntity, object>> sort, bool desc);
        Task<IEnumerable<TEntity>> GetSorted(Expression<Func<TEntity, object>> sort, bool desc, int skip, int limit);
        Task<IEnumerable<TEntity>> GetSorted(Expression<Func<TEntity, bool>> search, Expression<Func<TEntity, object>> sort, bool desc);
        Task<IEnumerable<TEntity>> GetSorted(Expression<Func<TEntity, bool>> search, Expression<Func<TEntity, object>> sort, bool desc, int skip, int limit);

        Task<IEnumerable<TEntity>> GetIn<F>(Expression<Func<TEntity, F>> field, IEnumerable<F> values);
        Task<TEntity> GetInFirst<F>(Expression<Func<TEntity, F>> field, IEnumerable<F> values);
        Task<IEnumerable<TEntity>> GetInSorted<F>(Expression<Func<TEntity, F>> field, IEnumerable<F> values, Expression<Func<TEntity, object>> sort, bool desc);
        Task<IEnumerable<TEntity>> GetInSorted<F>(Expression<Func<TEntity, F>> field, IEnumerable<F> values, Expression<Func<TEntity, object>> sort, bool desc, int skip, int limit);

        Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> search);
        Task<TEntity> GetFirstSorted(Expression<Func<TEntity, bool>> search, Expression<Func<TEntity, object>> sort, bool desc);

        #endregion

        #region count

        Task<long> Count(Expression<Func<TEntity, bool>> search);
        Task<long> Count<F>(Expression<Func<TEntity, F>> field, F value);
        Task<bool> Any(Expression<Func<TEntity, bool>> search);
        Task<bool> Any<F>(Expression<Func<TEntity, F>> field, F value);
        Task<bool> ExistsOne<F>(Expression<Func<TEntity, F>> field, F value);
        Task<bool> ExistsOne(Expression<Func<TEntity, bool>> search);
        Task<bool> ExistsOne(TKey id);

        #endregion

        #region update

        Task<TEntity> Replace(TEntity entity);
        Task<long> Replace(IEnumerable<TEntity> entities);

        #endregion

        #region delete

        Task DeleteSoft(TEntity entity);
        Task DeleteSoft(TKey id);
        Task DeleteSoftFirst(Expression<Func<TEntity, bool>> search);
        Task DeleteSoftAll(Expression<Func<TEntity, bool>> search);

        Task DeleteHard(TEntity entity);
        Task DeleteHard(TKey id);
        Task DeleteHardFirst(Expression<Func<TEntity, bool>> search);
        Task DeleteHardAll(Expression<Func<TEntity, bool>> search);

        #endregion
    }

    /// <summary>
    /// Simple interpretation of IMRRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IMRRepository<TEntity> : IMRRepository<TEntity, string>
        where TEntity : class, IMREntity, new()
    {

    }
}
