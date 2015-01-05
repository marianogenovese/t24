//-----------------------------------------------------------------------
// <copyright file="ReaderWriterResourceLock.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Threading;
    
    /// <summary>
    /// A ResourceLock implemented using System.Threading.ReaderWriterLockSlim
    /// </summary>
    internal sealed class ReaderWriterResourceLock : ResourceLock
    {
        /// <summary>
        /// The object that provides the low level locking mechanism
        /// </summary>
        private readonly ReaderWriterLockSlim @lock;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderWriterResourceLock"/> class.
        /// </summary>
        public ReaderWriterResourceLock()
        {
            this.@lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }
        
        /// <inheritdoc />
        protected override void OnEnterLock(bool exclusive)
        {
            if (exclusive)
            {
                @lock.EnterWriteLock();
            }
            else
            {
                @lock.EnterReadLock();
            }
        }

        /// <inheritdoc />
        protected override void OnExitLock(bool exclusive)
        {
            if (exclusive)
            {
                @lock.ExitWriteLock();
            }
            else
            {
                @lock.ExitReadLock();
            }
        }
    }
}
