//-----------------------------------------------------------------------
// <copyright file="IBootEngineModule.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// This class allow to implement a queue module for scheduling a request.
    /// </summary>
    internal interface IBootEngineModule : IModule
    {        
        /// <summary>
        /// Contains the logic to boot the engine
        /// </summary>
        void BootEngine();
    }
}
