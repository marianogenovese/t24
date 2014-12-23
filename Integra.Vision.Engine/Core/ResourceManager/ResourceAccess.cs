//-----------------------------------------------------------------------
// <copyright file="ResourceAccess.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;

    /// <summary>
    /// Provides a common base implementation for requests for access to resources.
    /// </summary>
    internal abstract class ResourceAccess : IDisposable
    {
        /// <summary>
        /// To determine if access is exclusive.
        /// </summary>
        private readonly bool isExclusive = false;
        
        /// <summary>
        /// The related resource manager.
        /// </summary>
        private readonly ResourceManager manager;

        /// <summary>
        /// The token created to the access.
        /// </summary>
        private ResourceAccessToken token;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAccess"/> class.
        /// </summary>
        /// <param name="manager">The resource manager.</param>
        /// <param name="exclusive">True if the access is exclusive.</param>
        public ResourceAccess(ResourceManager manager, bool exclusive)
        {
            this.manager = manager;
            this.isExclusive = exclusive;
            this.LockResource();
        }

        /// <summary>
        /// Gets the resource manager.
        /// </summary>
        public ResourceManager Manager
        {
            get
            {
                return this.manager;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the resource access is exclusive.
        /// </summary>
        public bool IsExclusive
        {
            get
            {
                return this.isExclusive;
            }
        }

        /// <summary>
        /// Releases the resources related to the resource access.
        /// </summary>
        public virtual void Dispose()
        {
            this.token.Release();
        }

        /// <summary>
        /// Try to take the control of the resource, locking it.
        /// </summary>
        protected virtual void LockResource()
        {
            this.token = this.manager.LockResource(this.isExclusive);
        }
    }
}
