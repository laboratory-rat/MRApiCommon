using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MRApiCommon.Infrastructure.Attr;
using MRApiCommon.Infrastructure.Enum;
using MRApiCommon.Infrastructure.Interface;
using MRApiCommon.Options;
using MRApiCommon.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MRApiCommon.Infrastructure.Database
{
    public abstract class MRMongoRepository<TEntity, TKey> : IMRRepository<TEntity, TKey>
            where TEntity : class, IMREntity<TKey>, new()
    {
        protected IMongoClient _client { get; set; }
        protected IMongoDatabase _database { get; set; }
        protected IMongoCollection<TEntity> _collection { get; set; }

        public MRMongoRepository(IOptions<MRDbOptions> settings) : this(settings.Value.ConnectionString, settings.Value.Database) { }
        public MRMongoRepository(string connection, string database) : this(new MongoClient(connection), database) { }
        public MRMongoRepository(IMongoClient client, string database) : this(client, client.GetDatabase(database)) { }
        public MRMongoRepository(IMongoClient client, IMongoDatabase database)
        {
            _client = client;
            _database = database;

            var collectionAttr = (CollectionAttr)Attribute.GetCustomAttribute(typeof(TEntity), typeof(CollectionAttr));
            var collectionName = collectionAttr == null && string.IsNullOrWhiteSpace(collectionAttr?.Name) ? typeof(TEntity).Name : collectionAttr.Name;

            _collection = _database.GetCollection<TEntity>(collectionName);
        }

        public static R Factory<R>(string collection, string database)
            where R : MRMongoRepository<TEntity, TKey>, IMRRepository<TEntity, TKey> => (R)Activator.CreateInstance(typeof(R), new object[] { collection, database });

        protected virtual MongoQueryBuilder<TEntity, TKey> _builder => new MongoQueryBuilder<TEntity, TKey>();

        #region create

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            entity.GenerateKey();
            entity.CreateTime = DateTime.UtcNow;
            entity.UpdateTime = null;

            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> Insert(IEnumerable<TEntity> list)
        {
            if (list == null || !list.Any()) return new List<TEntity>();

            var modList = list.ToList();

            modList.ForEach((x) =>
            {
                x.GenerateKey();
                x.CreateTime = DateTime.UtcNow;
                x.UpdateTime = null;
            });

            await _collection.InsertManyAsync(modList);

            return modList;
        }

        #endregion

        #region get

        public virtual async Task<TEntity> Get(TKey id)
            => await _collection.Find(_builder.Eq(x => x.Id, id).Filter).FirstOrDefaultAsync();

        public virtual async Task<IEnumerable<TEntity>> Get(IEnumerable<TKey> ids)
            => await _collection.Find(_builder.In(x => x.Id, ids).Filter).ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> search)
            => await _collection.Find(_builder.Where(search).Filter).ToListAsync();

        public virtual async Task<TEntity> Get<F>(Expression<Func<TEntity, F>> field, F value)
             => await _collection.Find(_builder.Eq(field, value).Filter).FirstOrDefaultAsync();

        public virtual async Task<IEnumerable<TEntity>> GetSorted(Expression<Func<TEntity, object>> sort, bool desc)
            => await _collection.Find(x => true).Sort(_builder.Sorting(sort, desc).Sort).ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> GetSorted(Expression<Func<TEntity, object>> sort, bool desc, int skip, int limit)
            => await _collection.Find(x => true).Sort(_builder.Sorting(sort, desc).Sort).Skip(skip).Limit(limit).ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> GetSorted(Expression<Func<TEntity, bool>> search, Expression<Func<TEntity, object>> sort, bool desc)
            => await _collection.Find(_builder.Where(search).Filter).Sort(_builder.Sorting(sort, desc).Sort).ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> GetSorted(Expression<Func<TEntity, bool>> search, Expression<Func<TEntity, object>> sort, bool desc, int skip, int limit)
            => await _collection.Find(_builder.Where(search).Filter).Sort(_builder.Sorting(sort, desc).Sort).Skip(skip).Limit(limit).ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> GetIn<F>(Expression<Func<TEntity, F>> field, IEnumerable<F> values)
            => await _collection.Find(_builder.In(field, values).Filter).ToListAsync();

        public virtual async Task<TEntity> GetInFirst<F>(Expression<Func<TEntity, F>> field, IEnumerable<F> values)
            => await _collection.Find(_builder.In(field, values).Filter).FirstOrDefaultAsync();

        public virtual async Task<IEnumerable<TEntity>> GetInSorted<F>(Expression<Func<TEntity, F>> field, IEnumerable<F> values, Expression<Func<TEntity, object>> sort, bool desc)
            => await _collection.Find(_builder.In(field, values).Filter).Sort(_builder.Sorting(sort, desc).Sort).ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> GetInSorted<F>(Expression<Func<TEntity, F>> field, IEnumerable<F> values, Expression<Func<TEntity, object>> sort, bool desc, int skip, int limit)
            => await _collection.Find(_builder.In(field, values).Filter).Sort(_builder.Sorting(sort, desc).Sort).Skip(skip).Limit(limit).ToListAsync();

        public virtual async Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> search)
            => await _collection.Find(_builder.Where(search).Filter).FirstOrDefaultAsync();

        public virtual async Task<TEntity> GetFirstSorted(Expression<Func<TEntity, bool>> search, Expression<Func<TEntity, object>> sort, bool desc)
            => await _collection.Find(_builder.Where(search).Filter).Sort(_builder.Sorting(sort, desc).Sort).FirstOrDefaultAsync();

        protected virtual async Task<IEnumerable<TEntity>> GetByQuery(MongoQueryBuilder<TEntity, TKey> query)
        {
            if (query.Filter == null)
                query.Where(x => true);

            var fluent = _collection.Find(query.Filter);

            if (query.Sort != null)
                fluent = fluent.Sort(query.Sort).Skip(query.PropertySkip).Limit(query.PropertyLimit);

            if (query.Projection != null)
            {
                fluent = fluent.Project<TEntity>(query.Projection);
            }

            return await fluent.ToListAsync();
        }

        protected virtual async Task<TEntity> GetByQueryFirst(MongoQueryBuilder<TEntity, TKey> query)
        {
            if (query.Filter == null)
                query.Where(x => true);

            var fluent = _collection.Find(query.Filter);

            if (query.Sort != null)
                fluent = fluent.Sort(query.Sort).Skip(query.PropertySkip);

            if (query.Projection != null)
            {
                fluent = fluent.Project<TEntity>(query.Projection);
            }

            return await fluent.FirstOrDefaultAsync();
        }

        #endregion

        #region count

        public virtual async Task<long> Count(Expression<Func<TEntity, bool>> search)
            => await _collection.CountDocumentsAsync(_builder.Where(search).Filter);

        public virtual async Task<long> Count<F>(Expression<Func<TEntity, F>> field, F value)
            => await _collection.CountDocumentsAsync(_builder.Eq(field, value).Filter);

        public virtual async Task<long> Count(MongoQueryBuilder<TEntity, TKey> query)
            => await _collection.CountDocumentsAsync(query.Filter);

        public virtual async Task<bool> Any(Expression<Func<TEntity, bool>> search)
            => (await Count(search)) > 0;

        public virtual async Task<bool> Any<F>(Expression<Func<TEntity, F>> field, F value)
            => (await Count(field, value)) > 0;

        public virtual async Task<bool> Any(MongoQueryBuilder<TEntity, TKey> query)
            => (await Count(query)) > 0;

        public virtual async Task<bool> ExistsOne(Expression<Func<TEntity, bool>> search)
            => (await Count(search)) == 1;

        public virtual async Task<bool> ExistsOne<F>(Expression<Func<TEntity, F>> field, F value)
            => (await Count(field, value)) == 1;

        public virtual async Task<bool> ExistsOne(TKey id)
            => (await Count(x => x.Id, id)) == 1;

        public virtual async Task<bool> ExistsOne(MongoQueryBuilder<TEntity, TKey> query)
            => (await Count(query)) == 1;

        #endregion

        #region update

        public virtual async Task<TEntity> Replace(TEntity entity)
        {
            entity.UpdateTime = DateTime.UtcNow;
            await _collection.ReplaceOneAsync(_builder.Eq(x => x.Id, entity.Id).Filter, entity);
            return entity;
        }

        public virtual async Task<long> Replace(IEnumerable<TEntity> entities)
        {
            var tasks = new List<Task>();
            var list = entities?.Where(x => x != null).ToList() ?? new List<TEntity>();

            foreach (var entity in entities)
            {
                entity.UpdateTime = DateTime.UtcNow;
                tasks.Add(Replace(entity));
            }

            await Task.WhenAll(tasks);

            return list.Count;
        }

        protected async Task UpdateByQuery(MongoQueryBuilder<TEntity, TKey> query)
        {
            if (query.Filter == null)
                query.Where(x => true);

            query.UpdateSet(x => x.UpdateTime, DateTime.UtcNow);

            await _collection.UpdateOneAsync(query.Filter, query.Update);
        }

        protected async Task UpdateManyByQuery(MongoQueryBuilder<TEntity, TKey> query)
        {
            if (query.Filter == null)
                query.Where(x => true);

            query.UpdateSet(x => x.UpdateTime, DateTime.UtcNow);

            await _collection.UpdateManyAsync(query.Filter, query.Update);
        }

        #endregion

        #region delete

        public virtual async Task DeleteSoft(TEntity entity)
            => await DeleteSoft(entity.Id);

        public virtual async Task DeleteSoft(TKey id)
           => await UpdateByQuery(_builder.Eq(x => x.Id, id).UpdateSet(x => x.State, MREntityState.Archived));

        public virtual async Task DeleteSoftFirst(Expression<Func<TEntity, bool>> search)
            => await UpdateByQuery(_builder.Where(search).UpdateSet(x => x.State, MREntityState.Archived));

        public virtual async Task DeleteSoftAll(Expression<Func<TEntity, bool>> search)
            => await UpdateManyByQuery(_builder.Where(search).UpdateSet(x => x.State, MREntityState.Archived));

        public virtual async Task DeleteHard(TEntity entity)
            => await DeleteHard(entity.Id);

        public virtual async Task DeleteHard(TKey id)
            => await _collection.DeleteOneAsync(_builder.Eq(x => x.Id, id).Filter);

        public virtual async Task DeleteHardFirst(Expression<Func<TEntity, bool>> search)
            => await _collection.DeleteOneAsync(_builder.Where(search).Filter);

        public virtual async Task DeleteHardAll(Expression<Func<TEntity, bool>> search)
            => await _collection.DeleteManyAsync(_builder.Where(search).Filter);

        #endregion
    }

    public abstract class MRMongoRepository<TEntity> : MRMongoRepository<TEntity, string>, IMRRepository<TEntity, string>
        where TEntity : class, IMREntity, new()
    {
        public MRMongoRepository(IOptions<MRDbOptions> settings) : base(settings) { }
        public MRMongoRepository(string connection, string database) : base(connection, database) { }
        public MRMongoRepository(IMongoClient client, string database) : base(client, database) { }
        public MRMongoRepository(IMongoClient client, IMongoDatabase database) : base(client, database) { }

        public static R Factory<R>(string collection, string database)
           where R : MRMongoRepository<TEntity>, IMRRepository<TEntity> => (R)Activator.CreateInstance(typeof(R), new object[] { collection, database });
    }
}