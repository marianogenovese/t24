//-----------------------------------------------------------------------
// <copyright file="CreateStreamDependencyEnumerator.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateStream
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Contains dependency enumerator logic for Create Stream command
    /// </summary>
    internal sealed class CreateStreamDependencyEnumerator : IDependencyEnumerator
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
                int childrenCount = interpretedCommand.Plan.Root.Children.Count;
                if (childrenCount == 4)
                {
                    dependencies.Add(new CommandDependency(interpretedCommand.Plan.Root.Children[1].Children[0].Properties["Value"].ToString(), ObjectTypeEnum.Source));
                }
                else if (childrenCount == 7)
                {
                    dependencies.Add(new CommandDependency(interpretedCommand.Plan.Root.Children[1].Children[0].Properties["Value"].ToString(), ObjectTypeEnum.Source));
                    if (interpretedCommand.Plan.Root.Children[1].Children[0].Properties["Value"].ToString() != interpretedCommand.Plan.Root.Children[2].Children[0].Properties["Value"].ToString())
                    {
                        dependencies.Add(new CommandDependency(interpretedCommand.Plan.Root.Children[2].Children[0].Properties["Value"].ToString(), ObjectTypeEnum.Source));
                    }
                }
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
