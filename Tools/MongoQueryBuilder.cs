using MongoDB.Driver;
using MRApiCommon.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MRApiCommon.Tools
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class MongoQueryBuilder<TEntity, TKey>
        where TEntity : class, IMREntity<TKey>, new()
    {
        #region builders

        public FilterDefinitionBuilder<TEntity> FilterBuilder { get; set; } = new FilterDefinitionBuilder<TEntity>();
        public SortDefinitionBuilder<TEntity> SortBuilder { get; set; } = new SortDefinitionBuilder<TEntity>();
        public UpdateDefinitionBuilder<TEntity> UpdateBuilder { get; set; } = new UpdateDefinitionBuilder<TEntity>();
        public ProjectionDefinitionBuilder<TEntity> ProjectionBuilder { get; set; } = new ProjectionDefinitionBuilder<TEntity>();

        #endregion

        #region definitions

        public FilterDefinition<TEntity> Filter { get; set; }
        public SortDefinition<TEntity> Sort { get; set; }
        public UpdateDefinition<TEntity> Update { get; set; }
        public ProjectionDefinition<TEntity> Projection { get; set; }

        #endregion

        #region limits

        public int? PropertySkip { get; set; }
        public int? PropertyLimit { get; set; }
        public bool PropertyDesc { get; set; }

        #endregion

        #region filter

        public MongoQueryBuilder<TEntity, TKey> Where(Expression<Func<TEntity, bool>> search)
        {
            if (search == null)
                search = x => true;

            AddOrCreate(FilterBuilder.Where(search));
            return this;
        }
        public MongoQueryBuilder<TEntity, TKey> In<F>(Expression<Func<TEntity, F>> field, IEnumerable<F> values)
        {
            AddOrCreate(FilterBuilder.In(field, values));
            return this;
        }
        public MongoQueryBuilder<TEntity, TKey> Eq<F>(Expression<Func<TEntity, F>> field, F value)
        {
            AddOrCreate(FilterBuilder.Eq(field, value));
            return this;
        }
        public MongoQueryBuilder<TEntity, TKey> Match<F>(Expression<Func<TEntity, IEnumerable<F>>> list, Expression<Func<F, bool>> match)
        {
            AddOrCreate(FilterBuilder.ElemMatch(list, match));
            return this;
        }
        public MongoQueryBuilder<TEntity, TKey> Regex(Expression<Func<TEntity, object>> field, string pattern, string options = "")
        {
            AddOrCreate(FilterBuilder.Regex(field, new MongoDB.Bson.BsonRegularExpression(pattern, options)));
            return this;
        }

        #endregion

        #region sort

        public MongoQueryBuilder<TEntity, TKey> Limit(int? limit)
        {
            PropertyLimit = limit;
            return this;
        }

        public MongoQueryBuilder<TEntity, TKey> Skip(int? skip)
        {
            PropertySkip = skip;
            return this;
        }

        public MongoQueryBuilder<TEntity, TKey> Sorting(Expression<Func<TEntity, object>> sort, bool desc)
        {
            if (sort == null)
                sort = x => x.Id;

            if (Sort == null)
            {
                if (desc)
                    Sort = SortBuilder.Descending(sort);
                else
                    Sort = SortBuilder.Ascending(sort);
            }
            else
            {
                if (desc)
                    Sort = Sort.Descending(sort);
                else
                    Sort = Sort.Ascending(sort);
            }

            return this;
        }

        #endregion

        #region update

        public MongoQueryBuilder<TEntity, TKey> UpdateSet<F>(Expression<Func<TEntity, F>> field, F value)
        {
            if (Update == null)
                Update = UpdateBuilder.Set(field, value);
            else
                Update = Update.Set(field, value);

            return this;
        }

        public MongoQueryBuilder<TEntity, TKey> UpdateAddToSet<F>(Expression<Func<TEntity, IEnumerable<F>>> collection, F value)
        {
            if (Update == null)
                Update = UpdateBuilder.AddToSet(collection, value);
            else
                Update = Update.AddToSet(collection, value);

            return this;
        }

        public MongoQueryBuilder<TEntity, TKey> UpdateAddToSetEach<F>(Expression<Func<TEntity, IEnumerable<F>>> collection, IEnumerable<F> values)
        {
            if (Update == null)
                Update = UpdateBuilder.AddToSetEach(collection, values);
            else
                Update = Update.AddToSetEach(collection, values);

            return this;
        }

        public MongoQueryBuilder<TEntity, TKey> UpdatePush<F>(Expression<Func<TEntity, IEnumerable<F>>> collection, F value)
        {
            if (Update == null)
                Update = UpdateBuilder.Push(collection, value);
            else
                Update = Update.Push(collection, value);

            return this;
        }

        public MongoQueryBuilder<TEntity, TKey> UpdatePull<F>(Expression<Func<TEntity, IEnumerable<F>>> collection, F item)
        {
            if (Update == null)
                Update = UpdateBuilder.Pull(collection, item);
            else
                Update = Update.Pull(collection, item);

            return this;
        }

        public MongoQueryBuilder<TEntity, TKey> UpdatePull<F>(Expression<Func<TEntity, IEnumerable<F>>> collection, IEnumerable<F> values)
        {
            if (Update == null)
                Update = UpdateBuilder.PullAll(collection, values);
            else
                Update = Update.PullAll(collection, values);

            return this;
        }

        public MongoQueryBuilder<TEntity, TKey> UpdatePullWhere<F>(Expression<Func<TEntity, IEnumerable<F>>> collection, Expression<Func<F, bool>> search)
        {
            if (Update == null)
                Update = UpdateBuilder.PullFilter(collection, search);
            else
                Update = Update.PullFilter(collection, search);

            return this;
        }

        #endregion

        #region projection

        public MongoQueryBuilder<TEntity, TKey> ProjectionInclude(Expression<Func<TEntity, object>> field)
        {
            if (Projection == null)
                Projection = ProjectionBuilder.Include(field);
            else
                Projection = Projection.Include(field);

            return this;
        }
        public MongoQueryBuilder<TEntity, TKey> ProjectionExclude(Expression<Func<TEntity, object>> field)
        {
            if (Projection == null)
                Projection = ProjectionBuilder.Exclude(field);
            else
                Projection = Projection.Exclude(field);

            return this;
        }

        #endregion

        #region utilites

        public FilterDefinition<TEntity> And(FilterDefinition<TEntity> f1, FilterDefinition<TEntity> f2) => And(new FilterDefinition<TEntity>[] { f1, f2 });
        public FilterDefinition<TEntity> And(params FilterDefinition<TEntity>[] args) => FilterBuilder.And(args);
        public FilterDefinition<TEntity> Or(params FilterDefinition<TEntity>[] args) => FilterBuilder.Or(args);

        protected void AddOrCreate(FilterDefinition<TEntity> defenition)
        {
            if (Filter == null)
                Filter = defenition;
            else
                Filter = And(Filter, defenition);
        }

        #endregion

    }
}
