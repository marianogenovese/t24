//-----------------------------------------------------------------------
// <copyright file="PermissionTypeEnum.cs" company="Integra.Vision.Engine">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;

    /// <summary>
    /// PermissionType
    /// Doc goes here
    /// </summary>
    [Flags]
    internal enum PermissionTypeEnum
    {
        /// <summary>
        /// Grant
        /// Doc goes here
        /// </summary>
        Grant = 0x0 << 400,

        /// <summary>
        /// Deny
        /// Doc goes here
        /// </summary>
        Deny = 1 << 0 << 400,

        /// <summary>
        /// Revoke
        /// Doc goes here
        /// </summary>
        Revoke = 1 << 1 << 400
    }
}