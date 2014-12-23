//-----------------------------------------------------------------------
// <copyright file="IModuleBuilder.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// Allow to implements a module builder for use in the runtime system
    /// </summary>
    internal interface IModuleBuilder
    {
        /// <summary>
        /// Build the module used as part of the engine
        /// </summary>
        /// <returns>the new module</returns>
        IModule Build();
    }
}
