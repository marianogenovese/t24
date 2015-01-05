//-----------------------------------------------------------------------
// <copyright file="ResourceAccessWrapper.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Provides a common base implementation for requests for access to resources with a specific instance of resource.
    /// </summary>
    /// <typeparam name="T">the type of resource that has been requested access.</typeparam>
    internal sealed class ResourceAccessWrapper<T> : ResourceAccess<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceAccessWrapper{T}"/> class.
        /// </summary>
        /// <param name="manager">The resource manager.</param>
        /// <param name="exclusive">True if the access is exclusive.</param>
        internal ResourceAccessWrapper(ResourceManagerWrapper<T> manager, bool exclusive) : base(manager, manager.Resource, exclusive)
        {
        }
    }
}
