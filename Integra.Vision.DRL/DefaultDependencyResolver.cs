//-----------------------------------------------------------------------
// <copyright file="DefaultDependencyResolver.cs" company="Integra.Vision.Dependency">
//     Copyright (c) Integra.Vision.Dependency. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Dependency
{
    using System;
    using Microsoft.Practices.Unity;
    
    /// <summary>
    /// This class implements the default dependency resolver for the engine system with Unity.
    /// </summary>
    internal sealed class DefaultDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// Unity container for dependency resolution.
        /// </summary>
        private IUnityContainer container = new UnityContainer();

        /// <summary>
        /// Register a type.
        /// </summary>
        /// <typeparam name="T">Type this registration is for.</typeparam>
        public void RegisterType<T>()
        {
            this.container.RegisterType<T>();
        }

        /// <summary>
        /// Register a type mapping.
        /// </summary>
        /// <typeparam name="TFrom">System.Type that will be requested.</typeparam>
        /// <typeparam name="TTo">System.Type that will actually be returned.</typeparam>
        public void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            this.container.RegisterType<TFrom, TTo>();
        }

        /// <summary>
        /// Register a type for the given name.
        /// </summary>
        /// <typeparam name="T">Type this registration is for.</typeparam>
        /// <param name="name">Name that will be used to request the type.</param>
        public void RegisterType<T>(string name)
        {
            this.container.RegisterType<T>(name);
        }

        /// <summary>
        /// Register a type mapping.
        /// </summary>
        /// <typeparam name="TFrom">System.Type that will be requested.</typeparam>
        /// <typeparam name="TTo">System.Type that will actually be returned.</typeparam>
        /// <param name="name">Name of this mapping.</param>
        public void RegisterType<TFrom, TTo>(string name) where TTo : TFrom
        {
            this.container.RegisterType<TFrom, TTo>(name);
        }

        /// <summary>
        /// Register an instance.
        /// </summary>
        /// <typeparam name="T">Type of instance to register (may be an implemented interface instead of the full type).</typeparam>
        /// <param name="instance">Object to returned.</param>
        public void RegisterInstance<T>(T instance)
        {
            this.container.RegisterInstance<T>(instance);
        }

        /// <summary>
        /// Register an instance.
        /// </summary>
        /// <typeparam name="T">Type of instance to register (may be an implemented interface instead of the full type).</typeparam>
        /// <param name="name">Name for registration.</param>
        /// <param name="instance">Object to returned.</param>
        public void RegisterInstance<T>(string name, T instance)
        {
            this.container.RegisterInstance<T>(name, instance);
        }
        
        /// <summary>
        /// Resolve an instance of the requested type with the given name from the container.
        /// </summary>
        /// <typeparam name="T">System.Type of object to get from the container</typeparam>
        /// <param name="name">Name of the object to retrieve</param>
        /// <returns>The retrieved object</returns>
        public T Resolve<T>(string name)
        {
            return this.container.Resolve<T>(name);
        }

        /// <summary>
        /// Resolve an instance of the requested type with the given name from the container.
        /// </summary>
        /// <typeparam name="T">System.Type of object to get from the container.</typeparam>
        /// <returns>The retrieved object.</returns>
        public T Resolve<T>()
        {
            return this.container.Resolve<T>();
        }

        /// <summary>
        /// Check if a particular type/name pair has been registered.
        /// </summary>
        /// <typeparam name="T">Type to check registration for.</typeparam>
        /// <param name="name">Name to check registration for.</param>
        /// <returns>True if this type/name pair has been registered, false if not.</returns>
        public bool CanResolve<T>(string name)
        {
            return this.container.IsRegistered<T>(name);
        }

        /// <summary>
        /// Check if a particular type has been registered with the default name.
        /// </summary>
        /// <typeparam name="T">Type to check registration for.</typeparam>
        /// <returns>True if this type has been registered, false if not.</returns>
        public bool CanResolve<T>()
        {
            return this.container.IsRegistered<T>();
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.container.Dispose();
        }
    }
}
