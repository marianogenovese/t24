//-----------------------------------------------------------------------
// <copyright file="ApplicationExitCode.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host
{
    /// <summary>
    /// Internal return codes for application service execution
    /// </summary>
    internal enum ApplicationExitCode
    {
        /// <summary>
        /// Success application exit
        /// </summary>
        Success,

        /// <summary>
        /// Parsing input arguments error
        /// </summary>
        InputError,

        /// <summary>
        /// Execution error
        /// </summary>
        RuntimeError,

        /// <summary>
        /// Security validation error
        /// </summary>
        SecurityError
    }
}
