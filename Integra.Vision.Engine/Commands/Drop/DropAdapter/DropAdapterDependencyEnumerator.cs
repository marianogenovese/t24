//-----------------------------------------------------------------------
// <copyright file="DropAdapterDependencyEnumerator.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Drop.DropAdapter
{
    /// <summary>
    /// Contains dependency enumerator logic for Drop Adapter command
    /// </summary>
    internal sealed class DropAdapterDependencyEnumerator : IDependencyEnumerator
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
