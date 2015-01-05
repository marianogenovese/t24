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
        /// Adapter
        /// Doc goes here
        /// </summary>
        Adapter = 1,

        /// <summary>
        /// Source
        /// Doc goes here
        /// </summary>
        Source = 2,

        /// <summary>
        /// Stream
        /// Doc goes here
        /// </summary>
        Stream = 4,

        /// <summary>
        /// Trigger
        /// Doc goes here
        /// </summary>
        Trigger = 8,

        /// <summary>
        /// Assembly
        /// Doc goes here
        /// </summary>
        Assembly = 16,

        /// <summary>
        /// Role
        /// Doc goes here
        /// </summary>
        Role = 32,

        /// <summary>
        /// User
        /// Doc goes here
        /// </summary>
        User = 64,

        /// <summary>
        /// Engine
        /// Doc goes here
        /// </summary>
        Engine = 128,

        /// <summary>
        /// SpecificObject
        /// Doc goes here
        /// </summary>
        SpecificObject = 256
    }
}
