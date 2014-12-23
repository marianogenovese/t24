//-----------------------------------------------------------------------
// <copyright file="CommandServerModuleBuilder.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using Integra.Vision.Dependency;
    
    /// <summary>
    /// This class implements a module builder for create a new Command Server.
    /// </summary>
    internal sealed class CommandServerModuleBuilder : IModuleBuilder
    {
        /// <summary>
        /// The dependency container where are registered different components.
        /// </summary>
        private IDependencyResolver dependencyResolver;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandServerModuleBuilder"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency container.</param>
        public CommandServerModuleBuilder(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }
        
        /// <summary>
        /// Build a new Command Server.
        /// </summary>
        /// <returns>The server created.</returns>
        public IModule Build()
        {
            this.dependencyResolver.RegisterInstance<ICommandServerModule>(new CommandServerModule(this.dependencyResolver.Resolve<IOperationSchedulerModule>()));
            return this.dependencyResolver.Resolve<ICommandServerModule>();
        }
    }
}
