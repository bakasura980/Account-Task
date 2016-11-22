using Innexys.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innexys.Models
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {

        public GenericRepository()
            :this(new DataContext()) { }

        public GenericRepository(DataContext context)
        {
            this.DbContext = context;
            this.DbSet = this.DbContext.Set<T>();
        }

        protected DbSet<T> DbSet { get; set; }

        protected DbContext DbContext { get; set; }

        public virtual IQueryable<T> All()
        {
            return this.DbSet.AsQueryable();
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry entry = this.DbContext.Entry(entity);

            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Detached;
            }
            else
            {
                this.DbSet.Add(entity);
            }
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry entry = this.DbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }
            entry.State = EntityState.Modified;
        }

        public virtual int SaveChanges()
        {
            return this.DbContext.SaveChanges();
        }
    }
}
