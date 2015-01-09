//-----------------------------------------------------------------------
// <copyright file="AlterTriggerDependencyEnumerator.cs" company="Integra.Vision.Enigne">
//     Copyright (c) Integra.Vision.Enigne. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Contains dependency enumerator logic for alter Trigger command
    /// </summary>
    internal sealed class AlterTriggerDependencyEnumerator : IDependencyEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterTriggerDependencyEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterTriggerDependencyEnumerator(PlanNode node)
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

                dependencies.Add(new CommandDependency(this.node.Children[1].Properties["Value"].ToString(), ObjectTypeEnum.Stream));

                int childrenCount = this.node.Children.Count;
                if (childrenCount == 3)
                {
                    foreach (Integra.Vision.Language.PlanNode plan in this.node.Children[2].Children)
                    {
                        dependencies.Add(new CommandDependency(plan.Children[1].Properties["Value"].ToString(), ObjectTypeEnum.Adapter));
                    }
                }
                else if (childrenCount == 4)
                {
                    foreach (Integra.Vision.Language.PlanNode ifPlan in this.node.Children[3].Children)
                    {
                        if (ifPlan.NodeType == PlanNodeTypeEnum.Send)
                        {
                            dependencies.Add(new CommandDependency(ifPlan.Children[1].Properties["Value"].ToString(), ObjectTypeEnum.Adapter));
                        }
                        else
                        {
                            foreach (Integra.Vision.Language.PlanNode sendPlan in ifPlan.Children)
                            {
                                dependencies.Add(new CommandDependency(sendPlan.Children[1].Properties["Value"].ToString(), ObjectTypeEnum.Adapter));
                            }
                        }
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
