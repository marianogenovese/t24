//-----------------------------------------------------------------------
// <copyright file="ICompileFilterContext.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine
{
    using Integra.Vision.Engine.Commands;

    /// <summary>
    /// This interface implements a context used for compiling process.
    /// </summary>
    internal interface ICompileFilterContext
    {
        /// <summary>
        /// Gets or sets the commands of the context.
        /// </summary>
        CommandBase[] Commands { get; set; }
    }
}
