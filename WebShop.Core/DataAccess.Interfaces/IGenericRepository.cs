using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using WebShop.Core.Models;
using X.PagedList;

namespace WebShop.Core.DataAccess.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
         );

        Task<IPagedList<T>> GetPagedList(
            PageRequestParams pageRequestParams,
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
        );

        Task<T> Get(int id);

        Task<T> Get(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task Insert(T entity);

        Task InsertRange(IEnumerable<T> entities);

        Task Update(int id, T entity);

        Task Delete(int id);

        void DeleteRange(IEnumerable<T> entities);

        Task<bool> Exists(Expression<Func<T, bool>> expression);

        Task Save();
    }
}
