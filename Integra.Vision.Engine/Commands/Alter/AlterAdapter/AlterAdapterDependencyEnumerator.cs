//-----------------------------------------------------------------------
// <copyright file="AlterAdapterDependencyEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Alter.AlterAdapter
{
    /// <summary>
    /// Contains dependency enumerator logic for alter adapter command
    /// </summary>
    internal sealed class AlterAdapterDependencyEnumerator : IDependencyEnumerator
    {
        /// <summary>
        /// Dependency enumeration implementation
        /// </summary>
        /// <param name="command">Command in context used for enumerate their arguments</param>
        /// <returns>Collection of command arguments</returns>
        public CommandDependency[] Enumerate(CommandBase command)
        {
            try
            {
                System.Collections.Generic.List<CommandDependency> dependencies = new System.Collections.Generic.List<CommandDependency>();
                /*
                dependencies.Add(new CommandDependency(command.Plan.Root.Children[3].Properties["Value"].ToString(), ObjectTypeEnum.Assembly));
                */
                return dependencies.ToArray();
            }
            catch (System.Exception e)
            {
                throw new DependencyEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
