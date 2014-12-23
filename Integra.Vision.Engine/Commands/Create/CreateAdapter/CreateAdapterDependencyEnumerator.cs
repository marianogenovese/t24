//-----------------------------------------------------------------------
// <copyright file="CreateAdapterDependencyEnumerator.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateAdapter
{
    /// <summary>
    /// Contains dependency enumerator logic for Create Assembly command
    /// </summary>
    internal sealed class CreateAdapterDependencyEnumerator : IDependencyEnumerator
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
                    dependencies.Add(new CommandDependency(interpretedCommand.Plan.Root.Children[3].Properties["Value"].ToString(), ObjectTypeEnum.Assembly));
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
