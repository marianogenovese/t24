//-----------------------------------------------------------------------
// <copyright file="Engine.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;

    /// <summary>
    /// This class implements the Integra Vision Engine Module.
    /// </summary>
    internal sealed class Engine : Module, IEngine
    {
        /// <summary>
        /// Abort the module.
        /// </summary>
        protected override void OnAbort()
        {
        }

        /// <summary>
        /// Stop the module.
        /// </summary>
        protected override void OnStop()
        {
        }

        /// <summary>
        /// Start the module.
        /// </summary>
        protected override void OnStart()
        {
        }
    }
}
