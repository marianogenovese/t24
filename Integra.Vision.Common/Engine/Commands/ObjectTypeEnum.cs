//-----------------------------------------------------------------------
// <copyright file="ObjectTypeEnum.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;

    /// <summary>
    /// ObjectType
    /// Doc goes here
    /// </summary>
    [Flags]
    internal enum ObjectTypeEnum
    {
        /// <summary>
        /// Source
        /// Doc goes here
        /// </summary>
        Source = 1 << 1 << 500,

        /// <summary>
        /// Stream
        /// Doc goes here
        /// </summary>
        Stream = 1 << 2 << 500,

        /// <summary>
        /// Role
        /// Doc goes here
        /// </summary>
        Role = 1 << 5 << 500,

        /// <summary>
        /// User
        /// Doc goes here
        /// </summary>
        User = 1 << 6 << 500,

        /// <summary>
        /// Engine
        /// Doc goes here
        /// </summary>
        Engine = 1 << 7 << 500,

        /// <summary>
        /// SpecificObject
        /// Doc goes here
        /// </summary>
        SpecificObject = 1 << 8 << 500
    }
}
