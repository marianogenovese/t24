//-----------------------------------------------------------------------
// <copyright file="ResourceLock.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    
    /// <summary>
    /// Provides a common base implementation for the concurrent resource locking mechanisms.
    /// </summary>
    internal abstract class ResourceLock : IDisposable
    {
        /// <summary>
        /// Allows the calling thread to acquire the lock for writing or reading.
        /// </summary>
        /// <param name="exclusive">true for exclusive access.</param>
        public virtual void EnterLock(bool exclusive)
        {
            this.OnEnterLock(exclusive);
        }
        
        /// <summary>
        /// Allows the calling thread to release the lock.
        /// </summary>
        /// <param name="exclusive">true for exclusive access.</param>
        public virtual void ExitLock(bool exclusive)
        {
            this.OnExitLock(exclusive);
        }

        /// <summary>
        /// Releases all resources used by the lock
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Allow to implement the locking mechanisms.
        /// </summary>
        /// <param name="exclusive">true for exclusive access.</param>
        protected abstract void OnEnterLock(bool exclusive);

        /// <summary>
        /// Allow to implement the release locking mechanisms.
        /// </summary>
        /// <param name="exclusive">true for exclusive access.</param>
        protected abstract void OnExitLock(bool exclusive);

        /// <summary>
        /// Releases all resources used by the lock.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if it is being finalized.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
