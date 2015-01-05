//-----------------------------------------------------------------------
// <copyright file="StartStopActionEnum.cs" company="Integra.Vision.Engine">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;

    /// <summary>
    /// StartStopAction
    /// Doc goes here
    /// </summary>
    [Flags]
    internal enum StartStopActionEnum
    {
        /// <summary>
        /// Start
        /// Doc goes here
        /// </summary>
        Start = 0x0 << 200,

        /// <summary>
        /// Stop
        /// Doc goes here
        /// </summary>
        Stop = 1 << 0 << 200
    }
}