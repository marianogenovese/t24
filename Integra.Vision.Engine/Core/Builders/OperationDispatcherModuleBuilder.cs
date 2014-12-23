//-----------------------------------------------------------------------
// <copyright file="OperationDispatcherModuleBuilder.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using Integra.Vision.Dependency;
    
    /// <summary>
    /// This class implements a module builder for create a new Command Executor used as request handler.
    /// </summary>
    internal sealed class OperationDispatcherModuleBuilder : IModuleBuilder
    {
        /// <summary>
        /// The dependency container where are registered different components.
        /// </summary>
        private IDependencyResolver dependencyResolver;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationDispatcherModuleBuilder"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency container.</param>
        public OperationDispatcherModuleBuilder(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        /// <inheritdoc />
        public IModule Build()
        {
            this.dependencyResolver.RegisterInstance<IOperationDispatcherModule>(new OperationDispatcherModule(this.dependencyResolver.Resolve<Engine>()));
            return this.dependencyResolver.Resolve<IOperationDispatcherModule>();
        }
    }
}
