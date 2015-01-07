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
                dependencies.Add(new CommandDependency(interpretedCommand.Plan.Root.Children[1].Properties["Value"].ToString(), ObjectTypeEnum.Stream));

                int childrenCount = interpretedCommand.Plan.Root.Children.Count;
                if (childrenCount == 3)
                {
                    foreach (PlanNode plan in interpretedCommand.Plan.Root.Children[2].Children)
                    {
                        dependencies.Add(new CommandDependency(plan.Children[1].Properties["Value"].ToString(), ObjectTypeEnum.Adapter));
                    }
                }
                else if (childrenCount == 4)
                {
                    foreach (PlanNode ifPlan in interpretedCommand.Plan.Root.Children[3].Children)
                    {
                        if (ifPlan.NodeType == (uint)Integra.Vision.Language.PlanNodeTypeEnum.Send)
                        {
                            dependencies.Add(new CommandDependency(ifPlan.Children[1].Properties["Value"].ToString(), ObjectTypeEnum.Adapter));
                        }
                        else
                        {
                            foreach (PlanNode sendPlan in ifPlan.Children)
                            {
                                dependencies.Add(new CommandDependency(sendPlan.Children[1].Properties["Value"].ToString(), ObjectTypeEnum.Adapter));
                            }
                        }
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
