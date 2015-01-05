//-----------------------------------------------------------------------
// <copyright file="IDependencyResolver.cs" company="Integra.Vision.Common">
//     Copyright (c) Integra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Dependency
{
    using System;
    
    /// <summary>
    /// Provides a dependency injection resolver.
    /// </summary>
    internal interface IDependencyResolver : IDisposable
    {
        /// <summary>
        /// Register a type.
        /// </summary>
        /// <typeparam name="T">Type this registration is for.</typeparam>
        void RegisterType<T>();
        
        /// <summary>
        /// Register a type mapping.
        /// </summary>
        /// <typeparam name="TFrom">System.Type that will be requested.</typeparam>
        /// <typeparam name="TTo">System.Type that will actually be returned.</typeparam>
        void RegisterType<TFrom, TTo>() where TTo : TFrom;
        
        /// <summary>
        /// Register a type for the given name.
        /// </summary>
        /// <typeparam name="T">Type this registration is for.</typeparam>
        /// <param name="name">Name that will be used to request the type.</param>
        void RegisterType<T>(string name);
        
        /// <summary>
        /// Register a type mapping.
        /// </summary>
        /// <typeparam name="TFrom">System.Type that will be requested.</typeparam>
        /// <typeparam name="TTo">System.Type that will actually be returned.</typeparam>
        /// <param name="name">Name of this mapping.</param>
        void RegisterType<TFrom, TTo>(string name) where TTo : TFrom;
        
        /// <summary>
        /// Register an instance.
        /// </summary>
        /// <typeparam name="T">Type of instance to register (may be an implemented interface instead of the full type).</typeparam>
        /// <param name="instance">Object to returned.</param>
        void RegisterInstance<T>(T instance);
        
        /// <summary>
        /// Register an instance.
        /// </summary>
        /// <typeparam name="T">Type of instance to register (may be an implemented interface instead of the full type).</typeparam>
        /// <param name="name">Name for registration.</param>
        /// <param name="instance">Object to returned.</param>
        void RegisterInstance<T>(string name, T instance);        
        
        /// <summary>
        /// Resolve an instance of the requested type with the given name from the container.
        /// </summary>
        /// <typeparam name="T">System.Type of object to get from the container</typeparam>
        /// <param name="name">Name of the object to retrieve</param>
        /// <returns>The retrieved object</returns>
        T Resolve<T>(string name);

        /// <summary>
        /// Resolve an instance of the requested type with the given name from the container.
        /// </summary>
        /// <typeparam name="T">System.Type of object to get from the container.</typeparam>
        /// <returns>The retrieved object.</returns>
        T Resolve<T>();

        /// <summary>
        /// Check if a particular type/name pair has been registered.
        /// </summary>
        /// <typeparam name="T">Type to check registration for.</typeparam>
        /// <param name="name">Name to check registration for.</param>
        /// <returns>True if this type/name pair has been registered, false if not.</returns>
        bool CanResolve<T>(string name);

        /// <summary>
        /// Check if a particular type has been registered with the default name.
        /// </summary>
        /// <typeparam name="T">Type to check registration for.</typeparam>
        /// <returns>True if this type has been registered, false if not.</returns>
        bool CanResolve<T>();
    }
}
