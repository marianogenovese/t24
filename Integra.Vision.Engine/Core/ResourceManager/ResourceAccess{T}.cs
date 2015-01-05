//-----------------------------------------------------------------------
// <copyright file="ResourceAccess{T}.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Provides a common base implementation for requests for access to resources with a specific instance of resource.
    /// </summary>
    /// <typeparam name="T">the type of resource that has been requested access.</typeparam>
    internal abstract class ResourceAccess<T> : ResourceAccess
    {
        /// <summary>
        /// A instance of the protected resource.
        /// </summary>
        private readonly T resource;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAccess{T}"/> class.
        /// </summary>
        /// <param name="manager">The resource manager.</param>
        /// <param name="instance">the instance to attempt to access.</param>
        /// <param name="exclusive">True if the access is exclusive.</param>
        public ResourceAccess(ResourceManager manager, T instance, bool exclusive) : base(manager, exclusive)
        {
            this.resource = instance;
        }
        
        /// <summary>
        /// Gets the resource to which access has been requested.
        /// </summary>
        public T Resource
        {
            get
            {
                return this.resource;
            }
        }
    }
}
