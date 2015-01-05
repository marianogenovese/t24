//-----------------------------------------------------------------------
// <copyright file="CreateTriggerDependencyEnumerator.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateTrigger
{
    /// <summary>
    /// Contains dependency enumerator logic for Create Trigger command
    /// </summary>
    internal sealed class CreateTriggerDependencyEnumerator : IDependencyEnumerator
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

                    dependencies.Add(new CommandDependency(interpretedCommand.Plan.Root.Children[1].Properties["Value"].ToString(), ObjectTypeEnum.Stream));
                    
                    int childrenCount = interpretedCommand.Plan.Root.Children.Count;
                    if (childrenCount == 3)
                    {
                        foreach (Integra.Vision.Language.PlanNode plan in interpretedCommand.Plan.Root.Children[2].Children)
                        {
                            dependencies.Add(new CommandDependency(plan.Children[1].Properties["Value"].ToString(), ObjectTypeEnum.Adapter));
                        }
                    }
                    else if (childrenCount == 4)
                    {
                        foreach (Integra.Vision.Language.PlanNode ifPlan in interpretedCommand.Plan.Root.Children[3].Children)
                        {
                            if (ifPlan.NodeType == (uint)Integra.Vision.Language.PlanNodeTypeEnum.Send)
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
                catch (System.Exception e)
                {
                    throw new DependencyEnumerationException(Resources.SR.EnumerationException, e);
                }
            }

            return new CommandDependency[] { };
        }
    }
}
