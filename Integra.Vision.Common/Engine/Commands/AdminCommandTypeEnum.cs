//-----------------------------------------------------------------------
// <copyright file="AdminCommandTypeEnum.cs" company="Integra.Vision.Engine">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;

    /// <summary>
    /// AdminCommandType
    /// Doc goes here
    /// </summary>
    [Flags]
    internal enum AdminCommandTypeEnum : long
    {
        /// <summary>
        /// BootEngine type
        /// Doc goes here
        /// </summary>
        BootEngine = 1 << 0 << 600,

        /// <summary>
        /// LoadAssembly type
        /// Doc goes here
        /// </summary>
        LoadAssembly = 1 << 0 << 700
    }
}