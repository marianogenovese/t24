//-----------------------------------------------------------------------
// <copyright file="IArgumentEnumerator.cs" company="Integra.Vision">
//     Copyright (c) Integra.Vision. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    /// <summary>
    /// Define a logic for command argument enumeration
    /// </summary>
    internal interface IArgumentEnumerator
    {
        /// <summary>
        /// Enumerate arguments for a specific command
        /// </summary>
        /// <param name="command">Command in context used for enumerate their arguments</param>
        /// <returns>Collection of command arguments</returns>
        CommandArgument[] Enumerate(CommandBase command);
    }
}
