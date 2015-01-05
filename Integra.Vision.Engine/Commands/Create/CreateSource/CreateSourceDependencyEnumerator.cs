//-----------------------------------------------------------------------
// <copyright file="CreateSourceDependencyEnumerator.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateSource
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Contains dependency enumerator logic for Create Source command
    /// </summary>
    internal sealed class CreateSourceDependencyEnumerator : IDependencyEnumerator
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
                List<CommandDependency> dependencies = new List<CommandDependency>();
                /*
                dependencies.Add(new CommandDependency(interpretedCommand.Plan.Root.Children[1].Children[0].Properties["Value"].ToString(), ObjectTypeEnum.Adapter));
                */
                return dependencies.ToArray();
            }
            catch (Exception e)
            {
                throw new DependencyEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
