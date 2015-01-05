//-----------------------------------------------------------------------
// <copyright file="DependencyExtensions.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Integra.Vision.Dependency;
    
    /// <summary>
    /// Provides dependency extensions methods used as utility functions
    /// </summary>
    internal static class DependencyExtensions
    {
        /// <summary>
        /// Reflection invoker of the register method.
        /// </summary>
        private static MethodInfo registerTypeInvoker;
        
        /// <summary>
        /// Reflection invoker of the resolver method.
        /// </summary>
        private static MethodInfo resolverInvoker;

        /// <summary>
        /// Gets the invoker for the register type method.
        /// </summary>
        private static MethodInfo RegisterInvoker
        {
            get
            {
                if (registerTypeInvoker == null)
                {
                    foreach (MethodInfo info in typeof(IDependencyResolver).GetMethods())
                    {
                        if (string.Equals(info.Name, "RegisterType", StringComparison.OrdinalIgnoreCase) && info.IsGenericMethod && info.GetParameters().Length == 1)
                        {
                            if (info.GetGenericArguments().Length == 2)
                            {
                                registerTypeInvoker = info;
                                break;
                            }
                        }
                    }
                }

                return registerTypeInvoker;
            }
        }

        /// <summary>
        /// Gets the invoker for the resolver type method.
        /// </summary>
        private static MethodInfo ResolverInvoker
        {
            get
            {
                if (resolverInvoker == null)
                {
                    foreach (MethodInfo info in typeof(IDependencyResolver).GetMethods())
                    {
                        if (string.Equals(info.Name, "Resolve", StringComparison.OrdinalIgnoreCase) && info.IsGenericMethod && info.GetParameters().Length == 1)
                        {
                            if (info.GetGenericArguments().Length == 1)
                            {
                                resolverInvoker = info;
                                break;
                            }
                        }
                    }
                }

                return resolverInvoker;
            }
        }

        /// <summary>
        /// Allow to register dynamically a type to the system dependency resolver.
        /// </summary>
        /// <param name="resolver">The dependency resolver.</param>
        /// <param name="from">The from type</param>
        /// <param name="to">The to type</param>
        /// <param name="name">The name of the type</param>
        public static void InvokeRegisterType(this IDependencyResolver resolver, Type from, Type to, string name)
        {
            RegisterInvoker.MakeGenericMethod(from, to).Invoke(resolver, new object[] { name });
        }

        /// <summary>
        /// Allow to resolve dynamically a type with system dependency resolver.
        /// </summary>
        /// <param name="resolver">The dependency resolver.</param>
        /// <param name="type">The type to resolve.</param>
        /// <param name="name">The name of the type</param>
        /// <returns>A instance of the requested type.</returns>
        public static object InvokeResolver(this IDependencyResolver resolver, Type type, string name)
        {
            return ResolverInvoker.MakeGenericMethod(type).Invoke(resolver, new object[] { name });
        }
    }
}
