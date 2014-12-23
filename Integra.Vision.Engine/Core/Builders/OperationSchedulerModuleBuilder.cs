//-----------------------------------------------------------------------
// <copyright file="OperationSchedulerModuleBuilder.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using Integra.Vision.Dependency;
    
    /// <summary>
    /// This class implements a module builder for create a new Command Queue for scheduling.
    /// </summary>
    internal sealed class OperationSchedulerModuleBuilder : IModuleBuilder
    {
        /// <summary>
        /// The dependency container where are registered different components.
        /// </summary>
        private IDependencyResolver dependencyResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationSchedulerModuleBuilder"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency container.</param>
        public OperationSchedulerModuleBuilder(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }
        
        /// <summary>
        /// Build a new Command Queue.
        /// </summary>
        /// <returns>The queue created.</returns>
        public IModule Build()
        {
            this.dependencyResolver.RegisterInstance<IOperationSchedulerModule>(new OperationSchedulerModule(this.dependencyResolver.Resolve<IOperationDispatcherModule>()));
            return this.dependencyResolver.Resolve<IOperationSchedulerModule>();
        }
    }
}
