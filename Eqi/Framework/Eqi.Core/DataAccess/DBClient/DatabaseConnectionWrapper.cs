using System;
using System.Data.Common;
using System.Threading;

namespace Eqi.Core.DataAccess
{
    /// <summary>
    /// This is a small helper class used to manage closing a connection 
    /// in the presence of transaction pooling. We can't actually
    /// close the connection until everyone using it is done, thus, we
    /// need reference counting.
    /// </summary>
    /// <remarks>
    /// User code should not use this class directly - it's used internally
    /// by the authors of DAAB providers to manage connections when using
    /// the DAAB methods.
    /// </remarks>
    public class DatabaseConnectionWrapper : IDisposable
    {
        /// <summary>
        /// Reference count.
        /// </summary>
        private int refCount;

        /// <summary>
        /// Gets the underlying <see cref="DbConnection"/> we're managing.
        /// </summary>
        public DbConnection Connection { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionWrapper"/> class that wraps
        /// the given <paramref name="connection"/>.
        /// </summary>
        /// <param name="connection">Database connection to manage the lifetime of.</param>
        public DatabaseConnectionWrapper(DbConnection connection)
        {
            this.Connection = connection;
            this.refCount = 1;
        }

        /// <summary>
        /// Gets a value indicating whether this wrapper has disposed the underlying connection.
        /// </summary>
        public bool IsDisposed
        {
            get { return this.refCount == 0; }
        }

        /// <summary>
        /// Increment the reference count for the wrapped connection.
        /// </summary>
        /// <returns>Database connection wrapper.</returns>
        public DatabaseConnectionWrapper AddRef()
        {
            Interlocked.Increment(ref this.refCount);
            return this;
        }

        #region IDisposable Members

        /// <summary>
        /// Decrement the reference count and, if refcount is 0, close the underlying connection.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Usual Dispose pattern folderal to shut up FxCop.
        /// </summary>
        /// <param name="disposing">True if called via <see cref="DatabaseConnectionWrapper.Dispose()"/> method, false
        /// if called from finalizer. Of course, since we have no finalizer this will never
        /// be false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                int count = Interlocked.Decrement(ref this.refCount);
                if (count == 0)
                {
                    this.Connection.Dispose();
                    this.Connection = null;
                    GC.SuppressFinalize(this);
                }
            }
        }

        #endregion
    }
}
