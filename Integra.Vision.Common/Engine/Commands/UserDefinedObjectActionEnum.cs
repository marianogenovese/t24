//-----------------------------------------------------------------------
// <copyright file="UserDefinedObjectActionEnum.cs" company="Integra.Vision.Engine">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;

    /// <summary>
    /// CommandUserDefineObjectAction
    /// Doc goes here
    /// </summary>
    [Flags]
    internal enum UserDefinedObjectActionEnum : long
    {
        /// <summary>
        /// Create
        /// Doc goes here
        /// </summary>
        Create = 1 << 0 << 100,

        /// <summary>
        /// Alter
        /// Doc goes here
        /// </summary>
        Alter = 1 << 1 << 100,

        /// <summary>
        /// Drop
        /// Doc goes here
        /// </summary>
        Drop = 1 << 2 << 100
    }
}