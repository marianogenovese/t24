//-----------------------------------------------------------------------
// <copyright file="SystemQueriesDependencyEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.SystemViews
{
    /// <summary>
    /// Contains dependency enumerator logic for the system views
    /// </summary>
    internal sealed class SystemQueriesDependencyEnumerator : IDependencyEnumerator
    {
        /// <summary>
        /// Dependency enumeration implementation
        /// </summary>
        /// <param name="command">Command in context used for enumerate their arguments</param>
        /// <returns>Collection of command arguments</returns>
        public CommandDependency[] Enumerate(CommandBase command)
        {
            if (command is InterpretedCommandBase)
            {
                try
                {
                    System.Collections.Generic.List<CommandDependency> dependencies = new System.Collections.Generic.List<CommandDependency>();
                    InterpretedCommandBase interpretedCommand = command as InterpretedCommandBase;
                    return dependencies.ToArray();
                }
                catch (System.Exception e)
                {
                    throw new DependencyEnumerationException(Resources.SR.EnumerationException, e);
                }
            }

            return new CommandDependency[] { };
        }
    }
}
