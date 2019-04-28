using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Service
{
    public interface IAsyncRepository<T> where T:class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
    }
}
