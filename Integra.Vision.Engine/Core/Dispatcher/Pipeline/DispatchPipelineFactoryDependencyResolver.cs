//-----------------------------------------------------------------------
// <copyright file="DispatchPipelineFactoryDependencyResolver.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Integra.Vision.Dependency;
    
    /// <summary>
    /// Provides dependency resolution for dispatch pipeline filters.
    /// </summary>
    internal sealed class DispatchPipelineFactoryDependencyResolver
    {
        /// <summary>
        /// Static instance.
        /// </summary>
        private static DispatchPipelineFactoryDependencyResolver instance;

        /// <summary>
        /// The container of dependencies.
        /// </summary>
        private IDependencyResolver dependencyResolver;

        /// <summary>
        /// List of registered factories.
        /// </summary>
        private Dictionary<string, Type> registersFactories = new Dictionary<string, Type>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchPipelineFactoryDependencyResolver"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency container.</param>
        public DispatchPipelineFactoryDependencyResolver(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        /// <summary>
        /// Gets the default instance of the resolver.
        /// </summary>
        public static DispatchPipelineFactoryDependencyResolver Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DispatchPipelineFactoryDependencyResolver(DependencyResolver.Default);
                }

                return instance;
            }
        }

        /// <summary>
        /// Allow to register a factory filter.
        /// </summary>
        /// <param name="factoryName">The filter factory name.</param>
        /// <param name="factoryType">The type of the factory.</param>
        public void RegisterDispatchFilterFactory(string factoryName, Type factoryType)
        {
            Type[] args = factoryType.GetDispatchFilterFactoryArgs();
            if (args == null)
            {
                throw new InvalidOperationException(Resources.SR.InvalidDispatchFilterFactoryType(factoryType.FullName));
            }
            
            this.dependencyResolver.InvokeRegisterType(typeof(DispatchFilterFactory<,>).MakeGenericType(args), factoryType, factoryName);
            this.registersFactories.Add(factoryName, typeof(DispatchFilterFactory<,>).MakeGenericType(args));
        }

        /// <summary>
        /// Resolve all registered factories in the dependency container.
        /// </summary>
        /// <returns>An array of instances.</returns>
        public object[] ResolveFactories()
        {
            List<object> instances = new List<object>();

            foreach (KeyValuePair<string, Type> element in this.registersFactories)
            {
                instances.Add(this.dependencyResolver.InvokeResolver(element.Value, element.Key));
            }

            return instances.ToArray();
        }
    }
}
