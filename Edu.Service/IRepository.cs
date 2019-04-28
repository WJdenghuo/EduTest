using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Edu.Service
{
    public interface IRepository<T> where T:class
    {
        T GetByID(int id);
        T Get(Expression<Func<T, bool>> whereExpression);
        IEnumerable<T> ListAll();
        ISugarQueryable<T> GetSugarQueryable(Expression<Func<T, bool>> filter = null);
        List<T> GetList(Expression<Func<T, bool>> whereExpression);
        T Add(T entity);
        Boolean Update(T entity);
        Boolean Delete(T entity);
        Boolean DeleteByID(dynamic id);
    }
}
