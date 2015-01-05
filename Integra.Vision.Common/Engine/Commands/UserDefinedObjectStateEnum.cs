//-----------------------------------------------------------------------
// <copyright file="UserDefinedObjectStateEnum.cs" company="Integra.Vision.Common">
//     Copyright (c) Integra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;

    /// <summary>
    /// User defined object state
    /// Doc goes here
    /// </summary>
    [Flags]
    internal enum UserDefinedObjectStateEnum
    {
        /// <summary>
        /// Stopped state
        /// </summary>
        Stopped = 0,

        /// <summary>
        /// Started state
        /// </summary>
        Started = 1,

        /// <summary>
        /// Stopped by error state
        /// </summary>
        StoppedByError = 2
    }
}
