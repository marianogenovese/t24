//-----------------------------------------------------------------------
// <copyright file="IEngineBuilder.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using Integra.Vision.Dependency;
    
    /// <summary>
    /// Allow implement the engine builder used in the engine bootstrap process.
    /// </summary>
    internal interface IEngineBuilder
    {
        /// <summary>
        /// This method is called in the bootstrap process of the engine system.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver used for register components.</param>
        void Build(IDependencyResolver dependencyResolver);
    }
}
