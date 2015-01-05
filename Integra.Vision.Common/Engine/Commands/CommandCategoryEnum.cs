//-----------------------------------------------------------------------
// <copyright file="CommandCategoryEnum.cs" company="Integra.Vision.Engine">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;

    /// <summary>
    /// CommandCategory
    /// Doc goes here
    /// </summary>
    [Flags]
    internal enum CommandCategoryEnum : long
    {
        /// <summary>
        /// UserDefinedObjectType
        /// Doc goes here
        /// </summary>
        UserDefinedObjectType = 0x0 << 10,

        /// <summary>
        /// PermissionType
        /// Doc goes here
        /// </summary>
        PermissionType = 1 << 0 << 10,

        /// <summary>
        /// SetType
        /// Doc goes here
        /// </summary>
        SetType = 1 << 1 << 10,

        /// <summary>
        /// SystemQueriesType
        /// Doc goes here
        /// </summary>
        SystemQueriesType = 1 << 2 << 10,

        /// <summary>
        /// StartStopType
        /// Doc goes here
        /// </summary>
        StartStopType = 1 << 3 << 10
    }
}