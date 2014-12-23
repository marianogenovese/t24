//-----------------------------------------------------------------------
// <copyright file="CreateTriggerArgumentEnumerator.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateTrigger
{
    /// <summary>
    /// Contains argument enumerator logic for Create Trigger command
    /// </summary>
    internal sealed class CreateTriggerArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Argument enumeration implementation
        /// </summary>
        /// <param name="command">Command in context used for enumerate their arguments</param>
        /// <returns>Collection of command arguments</returns>
        public CommandArgument[] Enumerate(CommandBase command)
        {
            if (command is InterpretedCommandBase)
            {
                System.Collections.Generic.List<CommandArgument> arguments = new System.Collections.Generic.List<CommandArgument>();
                InterpretedCommandBase interpretedCommand = command as InterpretedCommandBase;

                System.Collections.Generic.List<System.Tuple<string, System.Collections.Generic.List<string>>> ifList = new System.Collections.Generic.List<System.Tuple<string, System.Collections.Generic.List<string>>>();
                System.Collections.Generic.List<string> sendList = new System.Collections.Generic.List<string>();

                try
                {
                    arguments.Add(new CommandArgument("Name", interpretedCommand.Plan.Root.Children[0].Properties["Value"].ToString()));
                    arguments.Add(new CommandArgument("StreamName", interpretedCommand.Plan.Root.Children[1].Properties["Value"].ToString()));

                    int childrenCount = interpretedCommand.Plan.Root.Children.Count;
                    if (childrenCount == 3)
                    {
                        foreach (Integra.Vision.Language.PlanNode plan in interpretedCommand.Plan.Root.Children[2].Children)
                        {
                            sendList.Add(plan.Children[1].Properties["Value"].ToString());
                        }

                        arguments.Add(new CommandArgument("SendList", sendList));
                    }
                    else if (childrenCount == 4)
                    {
                        arguments.Add(new CommandArgument("ApplyWindow", interpretedCommand.Plan.Root.Children[2].Children[0].Properties["Value"]));

                        foreach (Integra.Vision.Language.PlanNode ifPlan in interpretedCommand.Plan.Root.Children[3].Children)
                        {
                            sendList = new System.Collections.Generic.List<string>();
                            if (ifPlan.NodeType == (uint)Integra.Vision.Language.PlanNodeTypeEnum.Send)
                            {
                                sendList.Add(ifPlan.Children[1].Properties["Value"].ToString());
                            }
                            else
                            {
                                foreach (Integra.Vision.Language.PlanNode sendPlan in ifPlan.Children)
                                {
                                    sendList.Add(sendPlan.Children[1].Properties["Value"].ToString());
                                }
                            }

                            ifList.Add(new System.Tuple<string, System.Collections.Generic.List<string>>(ifPlan.NodeType.ToString(), sendList));
                        }

                        arguments.Add(new CommandArgument("IfList", ifList));
                    }

                    return arguments.ToArray();
                }
                catch (System.Exception e)
                {
                    throw new ArgumentEnumerationException(Resources.SR.EnumerationException, e);
                }
            }

            return new CommandArgument[] { };
        }
    }
}
