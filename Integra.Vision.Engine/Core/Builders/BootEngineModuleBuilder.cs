//-----------------------------------------------------------------------
// <copyright file="BootEngineModuleBuilder.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using Integra.Vision.Dependency;

    /// <summary>
    /// This class implements a module builder for create a new Private Command Executor used as a private request handler.
    /// </summary>
    internal sealed class BootEngineModuleBuilder : IModuleBuilder
    {
        /// <summary>
        /// The dependency container where are registered different components.
        /// </summary>
        private IDependencyResolver dependencyResolver;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BootEngineModuleBuilder"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency container.</param>
        public BootEngineModuleBuilder(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        /// <inheritdoc />
        public IModule Build()
        {
            this.dependencyResolver.RegisterInstance<IBootEngineModule>(new BootEngineModule(this.dependencyResolver.Resolve<IOperationSchedulerModule>()));
            return this.dependencyResolver.Resolve<IBootEngineModule>();
        }
    }
}
