//-----------------------------------------------------------------------
// <copyright file="CommandAccessLevelEnum.cs" company="Integra.Vision.Engine">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;

    /// <summary>
    /// CommandAccessLevel
    /// Doc goes here
    /// </summary>
    [Flags]
    internal enum CommandAccessLevelEnum : long
    {
        /// <summary>
        /// Private
        /// Doc goes here
        /// </summary>
        Private = 0x0,
        
        /// <summary>
        /// Public
        /// Doc goes here
        /// </summary>
        Public = 1 << 0
    }
}