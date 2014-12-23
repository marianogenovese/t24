//-----------------------------------------------------------------------
// <copyright file="CommandActionPipelineFactoryDependencyResolver.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Integra.Vision.Dependency;
    using Integra.Vision.Engine.Commands;
    
    /// <summary>
    /// Provides dependency resolution for action pipeline filters.
    /// </summary>
    internal sealed class CommandActionPipelineFactoryDependencyResolver
    {
        /// <summary>
        /// Static instance.
        /// </summary>
        private static CommandActionPipelineFactoryDependencyResolver instance;
        
        /// <summary>
        /// The container of dependencies.
        /// </summary>
        private IDependencyResolver dependencyResolver;

        /// <summary>
        /// List of registered factories.
        /// </summary>
        private Dictionary<CommandTypeEnum, List<Type>> registersFactories = new Dictionary<CommandTypeEnum, List<Type>>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandActionPipelineFactoryDependencyResolver"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency container.</param>
        public CommandActionPipelineFactoryDependencyResolver(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        /// <summary>
        /// Gets the default instance of the resolver.
        /// </summary>
        public static CommandActionPipelineFactoryDependencyResolver Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommandActionPipelineFactoryDependencyResolver(DependencyResolver.Default);
                }

                return instance;
            }
        }

        /// <summary>
        /// Allow to register a factory filter.
        /// </summary>
        /// <param name="commandType">The filter factory name.</param>
        /// <param name="factoryType">The type of the factory.</param>
        public void RegisterActionFilterFactory(CommandTypeEnum commandType, Type factoryType)
        {
            this.dependencyResolver.InvokeRegisterType(factoryType, factoryType, commandType.GetStringRepresentation());
            if (!this.registersFactories.ContainsKey(commandType))
            {
                this.registersFactories.Add(commandType, new List<Type>());
            }

            this.registersFactories[commandType].Add(factoryType);
        }

        /// <summary>
        /// Resolve all registered factories in the dependency container by command type.
        /// </summary>        
        /// <param name="commandType">The command type.</param>
        /// <returns>An array of instances.</returns>
        public object[] ResolveFactories(CommandTypeEnum commandType)
        {
            List<object> instances = new List<object>();

            foreach (Type type in this.registersFactories[commandType])
            {
                instances.Add(this.dependencyResolver.InvokeResolver(type, commandType.GetStringRepresentation()));
            }

            return instances.ToArray();
        }
    }
}
