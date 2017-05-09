using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApiTesting.DataConnection
{
    public interface IRepository<T> where T : class
    {
        ICollection<T> All();

        void Add(T entity);

        void Update(T entity);

        T GetExactly(Expression<Func<T, bool>> value);

        int SaveChanges();
    }
}