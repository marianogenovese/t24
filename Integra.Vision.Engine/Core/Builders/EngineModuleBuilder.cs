//-----------------------------------------------------------------------
// <copyright file="EngineModuleBuilder.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using Integra.Vision.Dependency;
    
    /// <summary>
    /// This class implements a module builder for create a new Engine Module.
    /// </summary>
    internal sealed class EngineModuleBuilder : IModuleBuilder
    {
        /// <summary>
        /// The dependency container where are registered different components.
        /// </summary>
        private IDependencyResolver dependencyResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineModuleBuilder"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency container.</param>
        public EngineModuleBuilder(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }
        
        /// <summary>
        /// Build a new Engine.
        /// </summary>
        /// <returns>A engine created.</returns>
        public IModule Build()
        {
            this.dependencyResolver.RegisterInstance<IEngine>(new Engine());
            return this.dependencyResolver.Resolve<IEngine>();
        }
    }
}
