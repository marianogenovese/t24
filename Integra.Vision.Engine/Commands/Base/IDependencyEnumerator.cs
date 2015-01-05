//-----------------------------------------------------------------------
// <copyright file="IDependencyEnumerator.cs" company="Integra.Vision">
//     Copyright (c) Integra.Vision. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    /// <summary>
    /// Define a logic for command dependency enumeration
    /// </summary>
    internal interface IDependencyEnumerator
    {
        /// <summary>
        /// Enumerate dependencies for a specific command
        /// </summary>
        /// <param name="command">Command in context used for enumerate their dependencies</param>
        /// <returns>Collection of command dependencies</returns>
        CommandDependency[] Enumerate(CommandBase command);
    }
}
