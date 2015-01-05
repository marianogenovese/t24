//-----------------------------------------------------------------------
// <copyright file="ResourceAccessToken.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    
    /// <summary>
    /// Provides a state holder for the lock of the resource access.
    /// </summary>
    internal sealed class ResourceAccessToken : IDisposable
    {
        /// <summary>
        /// Is exclusive resource access.
        /// </summary>
        private readonly bool exclusive;
        
        /// <summary>
        /// The lock used for request access.
        /// </summary>
        private readonly ResourceLock @lock;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAccessToken"/> class.
        /// </summary>
        /// <param name="lock">The lock used for request access.</param>
        /// <param name="exclusive">Is exclusive resource access.</param>
        public ResourceAccessToken(ResourceLock @lock, bool exclusive)
        {
            this.@lock = @lock;
            this.exclusive = exclusive;
            this.@lock.EnterLock(this.exclusive);
        }

        /// <summary>
        /// Release the lock.
        /// </summary>
        public void Release()
        {
            this.Dispose();
        }

        /// <summary>
        /// Free and release the lock.
        /// </summary>
        public void Dispose()
        {
            this.@lock.ExitLock(this.exclusive);
        }
    }
}
