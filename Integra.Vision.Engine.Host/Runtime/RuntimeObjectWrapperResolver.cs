//-----------------------------------------------------------------------
// <copyright file="RuntimeObjectWrapperResolver.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host.Runtime
{
    using System;
    using System.Diagnostics.Contracts;
    using Integra.Vision.Dependency;
    
    /// <summary>
    /// This class implements a dependency resolver and object wrapper creation.
    /// </summary>
    internal static class RuntimeObjectWrapperResolver
    {
        /// <summary>
        /// Try to resolve and create a instance of the requested type and return a wrapper of it.
        /// </summary>
        /// <typeparam name="T">Type of wrapper.</typeparam>
        /// <param name="type">Name of the type used for create a wrapper</param>
        /// <param name="instance">When this method returns, contains the instance of the request wrapper if the type is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the request type exists; otherwise, false.</returns>
        public static bool TryResolve<T>(string type, out T instance) where T : RuntimeObjectWrapper
        {
            Contract.Requires(!string.IsNullOrEmpty(type));
            instance = default(T);
            object genericInstance = default(object);
            if (!RuntimeDependencyResolver.TryResolve(type, out genericInstance))
            {
                return false;
            }

            instance = Activator.CreateInstance(typeof(T), new object[] { genericInstance }) as T;
            return true;
        }
        
        /// <summary>
        /// Try to resolve and create a instance of the requested type and return a wrapper of it.
        /// </summary>
        /// <typeparam name="T">Type of wrapper.</typeparam>
        /// <param name="dependencyResolver">Used for resolve the requested type.</param>
        /// <param name="type">Name of the type used for create a wrapper</param>
        /// <param name="instance">When this method returns, contains the instance of the request wrapper if the type is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the request type exists; otherwise, false.</returns>
        public static bool TryResolve<T>(IDependencyResolver dependencyResolver, string type, out T instance) where T : RuntimeObjectWrapper
        {
            Contract.Requires(dependencyResolver != null);
            Contract.Requires(!string.IsNullOrEmpty(type));
            instance = default(T);

            Type requestedType = default(Type);
            if (!RuntimeDependencyResolver.TryResolveType(type, out requestedType))
            {
                return false;
            }

            bool canResolve = false;

            try
            {
                canResolve = (bool)dependencyResolver.GetType().GetMethod("CanResolve", Type.EmptyTypes).MakeGenericMethod(requestedType).Invoke(dependencyResolver, null);
            }
            catch
            {
            }

            if (!canResolve)
            {
                return canResolve;
            }

            instance = default(T);
            object resolvedInstance = default(object);

            try
            {
                resolvedInstance = dependencyResolver.GetType().GetMethod("Resolve", Type.EmptyTypes).MakeGenericMethod(requestedType).Invoke(dependencyResolver, null);
            }
            catch
            {
            }

            if (resolvedInstance == null)
            {
                return false;
            }

            instance = Activator.CreateInstance(typeof(T), new object[] { resolvedInstance }) as T;
            return true;
        }
    }
}
