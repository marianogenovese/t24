//-----------------------------------------------------------------------
// <copyright file="ResourceManagerWrapper.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Implements a concurrent resource management with a specific resource instance.
    /// </summary>
    /// <typeparam name="T">The type of the resource to be protected.</typeparam>
    internal sealed class ResourceManagerWrapper<T> : ResourceManager
    {
        /// <summary>
        /// An instance of the resource.
        /// </summary>
        private readonly T resource;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerWrapper{T}"/> class.
        /// </summary>
        /// <param name="instance">The instance of the resource to be protected.</param>
        public ResourceManagerWrapper(T instance) : base(new ReaderWriterResourceGate())
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
