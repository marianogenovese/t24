//-----------------------------------------------------------------------
// <copyright file="ResourceManager.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Provides a common base implementation for the concurrent resource management.
    /// </summary>
    internal abstract class ResourceManager : IDisposable
    {
        /// <summary>
        /// A dictionary with resources managers, that acts like a manager's cache.
        /// </summary>
        private static readonly Dictionary<Type, ResourceManager> RegisteredManagers = new Dictionary<Type, ResourceManager>();

        /// <summary>
        /// The gate that acts like a semaphore for the requests.
        /// </summary>
        private readonly ResourceGate gate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManager"/> class.
        /// </summary>
        /// <param name="gate">The gate that acts like a semaphore for the requests.</param>
        public ResourceManager(ResourceGate gate)
        {
            this.gate = gate;
        }

        /// <summary>
        /// Gets the gate which gives access to resources.
        /// </summary>
        protected virtual ResourceGate Gate
        {
            get
            {
                return this.gate;
            }
        }

        /// <summary>
        /// Registers the instance of a resource that must be protected
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">The instance.</param>
        public static void Register<T>(T instance)
        {
            RegisteredManagers.Add(typeof(T), new ResourceManagerWrapper<T>(instance));
        }
        
        /// <summary>
        /// Create a access used exclusively for writing.
        /// </summary>
        /// <typeparam name="T">The type of the instance to which access is.</typeparam>
        /// <returns>A new resource access.</returns>
        public static ResourceAccess<T> WriteAccess<T>()
        {
            return new ResourceAccessWrapper<T>((ResourceManagerWrapper<T>)RegisteredManagers[typeof(T)], true);
        }

        /// <summary>
        /// Create a access used for reading.
        /// </summary>
        /// <typeparam name="T">The type of the instance to which access is.</typeparam>
        /// <returns>A new resource access.</returns>
        public static ResourceAccess<T> ReadAccess<T>()
        {
            return new ResourceAccessWrapper<T>((ResourceManagerWrapper<T>)RegisteredManagers[typeof(T)], false);
        }

        /// <summary>
        /// Makes a request for access to a protected resource.
        /// </summary>
        /// <param name="exclusive">true for exclusive access.</param>
        /// <returns>A access token that hold the lock.</returns>
        public virtual ResourceAccessToken LockResource(bool exclusive)
        {
            return this.gate.LockResource(exclusive);
        }

        /// <summary>
        /// Releases the resources related to the resource manager.
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
