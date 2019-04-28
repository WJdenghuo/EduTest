using Edu.Service;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Entity
{
    public class SugarRepository<T> : SugarContent, IAsyncRepository<T>, IRepository<T> where T : class, new()
    {
        public T Add(T entity)
        {
            var test = new SimpleClient<T>(Db);
            if (test.Insert(entity))
                return entity;
            else
                return null;

        }

        public async Task<T> AddAsync(T entity)
        {
            var test = new SimpleClient<T>(Db);
            if (test.Insert(entity))
                return await Task.FromResult(entity);
            else
                return null;
        }

        public Boolean Delete(T entity)
        {
            var test = new SimpleClient<T>(Db);
            return test.Delete(entity);
        }

        public async void DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public T GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public T Get(Expression<Func<T, bool>> whereExpression)
        {
            var test = new SimpleClient<T>(Db);
            return test.GetSingle(whereExpression);
        }
        public List<T> GetList(Expression<Func<T, bool>> whereExpression)
        {
            var test = new SimpleClient<T>(Db);
            return test.GetList(whereExpression);
        }
        public async Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ListAll()
        {
            var test = new SimpleClient<T>(Db);
            return test.GetList();
        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {
            throw new NotImplementedException();
        }

        public Boolean Update(T entity)
        {
            var test = new SimpleClient<T>(Db);
            return test.Update(entity);
        }

        public async void UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public ISugarQueryable<T> GetSugarQueryable(Expression<Func<T, bool>> filter = null)
        {
            ISugarQueryable<T> query = Db.Queryable<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query;
        }

        public bool DeleteByID(dynamic id)
        {
            var test = new SimpleClient<T>(Db);
            if (id!=null)
            {
                return test.DeleteById(id);
            }
            return false;
        }
    }
}
