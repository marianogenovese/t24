//-----------------------------------------------------------------------
// <copyright file="AlterStreamDependencyEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Contains dependency enumerator logic for alter stream command
    /// </summary>
    internal sealed class AlterStreamDependencyEnumerator : IDependencyEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterStreamDependencyEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterStreamDependencyEnumerator(PlanNode node)
        {
            this.node = node;
        }

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

                int childrenCount = this.node.Children.Count;
                if (childrenCount == 4)
                {
                    dependencies.Add(new CommandDependency(this.node.Children[1].Children[0].Properties["Value"].ToString(), ObjectTypeEnum.Source));
                }
                else if (childrenCount == 7)
                {
                    dependencies.Add(new CommandDependency(this.node.Children[1].Children[0].Properties["Value"].ToString(), ObjectTypeEnum.Source));
                    if (this.node.Children[1].Children[0].Properties["Value"].ToString() != this.node.Children[2].Children[0].Properties["Value"].ToString())
                    {
                        dependencies.Add(new CommandDependency(this.node.Children[2].Children[0].Properties["Value"].ToString(), ObjectTypeEnum.Source));
                    }
                }

                return dependencies.ToArray();
            }
            catch (Exception e)
            {
                throw new DependencyEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
