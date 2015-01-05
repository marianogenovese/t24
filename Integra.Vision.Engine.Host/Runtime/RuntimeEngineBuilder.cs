//-----------------------------------------------------------------------
// <copyright file="RuntimeEngineBuilder.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host.Runtime
{
    using System.Diagnostics.Contracts;
    using Integra.Vision.Dependency;
    
    /// <summary>
    /// This class implements the engine bootstrap logic.
    /// </summary>
    internal sealed class RuntimeEngineBuilder : RuntimeObjectWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeEngineBuilder"/> class.
        /// </summary>
        /// <param name="instance">The engine builder used for build the engine.</param>
        public RuntimeEngineBuilder(object instance) : base(instance)
        {
            Contract.Requires<RuntimeException>(instance != null, Resources.SR.InvalidEngineBuilderRef);
        }

        /// <summary>
        /// Invoke to the build action in the Engine Builder.
        /// </summary>
        /// <param name="dependencyResolverInstance">The engine module dependency resolver.</param>
        public void Build(IDependencyResolver dependencyResolverInstance)
        {
            Contract.Requires(dependencyResolverInstance != null);
            if (!this.TryInvokeAction("Build", (IDependencyResolver)dependencyResolverInstance))
            {
                throw new RuntimeException(Resources.SR.UnableBuildEngine);
            }
        }
    }
}
