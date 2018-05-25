using System;
using System.Data;

namespace Eqi.Core.DataAccess.DBClient
{
    /// <summary>
    /// An implementation of <see cref="IDataReader"/> which also properly
    /// cleans up the reference count on the given inner <see cref="DatabaseConnectionWrapper"/>
    /// when the reader is closed or disposed.
    /// </summary>
    internal class RefCountingDataReader : DataReaderWrapper
    {
        /// <summary>
        /// Database connection wrapper.
        /// </summary>
        private readonly DatabaseConnectionWrapper connectionWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref='RefCountingDataReader'/> class that wraps
        /// the given <paramref name="innerReader"/> and properly
        /// cleans the refcount on the given <paramref name="connection"/>
        /// when done.
        /// </summary>
        /// <param name="connection">Connection to close.</param>
        /// <param name="innerReader">Reader to do the actual work.</param>
        public RefCountingDataReader(DatabaseConnectionWrapper connection, IDataReader innerReader)
            : base(innerReader)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (connection == null)
            {
                throw new ArgumentNullException("innerReader");
            }

            this.connectionWrapper = connection;
            this.connectionWrapper.AddRef();
        }

        /// <summary>
        /// Closes the <see cref="T:System.Data.IDataReader"/> Object.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Close()
        {
            if (!IsClosed)
            {
                base.Close();
                this.connectionWrapper.Dispose();
            }
        }

        /// <summary>
        /// Clean up resources.
        /// </summary>
        /// <param name="disposing">True if called from dispose, false if called from finalizer. We have no finalizer,
        /// so this will never be false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!IsClosed)
                {
                    base.Dispose(true);
                    this.connectionWrapper.Dispose();
                }
            }
        }
    }
}
