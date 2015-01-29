//-----------------------------------------------------------------------
// <copyright file="ActionTypeEnum.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;

    /// <summary>
    /// ActionTypeEnumerator
    /// Doc goes here
    /// </summary>
    [Flags]
    internal enum ActionTypeEnum
    {
        /// <summary>
        /// Publish
        /// Doc goes here
        /// </summary>
        Publish = 1 << 0 << 600,

        /// <summary>
        /// Receive
        /// Doc goes here
        /// </summary>
        Receive = 1 << 1 << 600
    }
}
