using Innexys.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innexys.Models
{
    public class UnitOfWork<T,U> : IDisposable where T : class where U : class
    {
        private DataContext context = new DataContext();

        private GenericRepository<T> tRepository;
        private GenericRepository<U> uRepository;

        public GenericRepository<T> TRepository
        {
            get
            {

                if (this.tRepository == null)
                {
                    this.tRepository = new GenericRepository<T>(context);
                }
                return tRepository;
            }
        }

        public GenericRepository<U> URepository
        {
            get
            {

                if (this.uRepository == null)
                {
                    this.uRepository = new GenericRepository<U>(context);
                }
                return uRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}