//-----------------------------------------------------------------------
// <copyright file="AlterTriggerArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Contains argument enumerator logic for alter Trigger command
    /// </summary>
    internal sealed class AlterTriggerArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterTriggerArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterTriggerArgumentEnumerator(PlanNode node)
        {
            this.node = node;
        }

        /// <summary>
        /// Argument enumeration implementation
        /// </summary>
        /// <param name="command">Command in context used for enumerate their arguments</param>
        /// <returns>Collection of command arguments</returns>
        public CommandArgument[] Enumerate(CommandBase command)
        {
            List<CommandArgument> arguments = new List<CommandArgument>();

            List<Tuple<string, string[]>> ifList = new List<Tuple<string, string[]>>();
            List<string> sendList = new List<string>();

            try
            {
                arguments.Add(new CommandArgument("Script", "create" + this.node.NodeText.Substring(5)));
                arguments.Add(new CommandArgument("Name", this.node.Children[0].Properties["Value"].ToString()));
                arguments.Add(new CommandArgument("StreamName", this.node.Children[1].Properties["Value"].ToString()));

                int childrenCount = this.node.Children.Count;
                if (childrenCount == 3)
                {
                    arguments.Add(new CommandArgument("IsSimpleTrigger", true));
                    foreach (Integra.Vision.Language.PlanNode plan in this.node.Children[2].Children)
                    {
                        sendList.Add(plan.Children[1].Properties["Value"].ToString());
                    }

                    arguments.Add(new CommandArgument("SendList", sendList.ToArray()));
                }
                else if (childrenCount == 4)
                {
                    arguments.Add(new CommandArgument("IsSimpleTrigger", false));
                    arguments.Add(new CommandArgument("ApplyWindow", this.node.Children[2].Children[0].Properties["Value"]));

                    foreach (Integra.Vision.Language.PlanNode ifPlan in this.node.Children[3].Children)
                    {
                        sendList = new System.Collections.Generic.List<string>();
                        if (ifPlan.NodeType == PlanNodeTypeEnum.Send)
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

                        ifList.Add(new System.Tuple<string, string[]>(ifPlan.NodeType.ToString(), sendList.ToArray()));
                    }

                    arguments.Add(new CommandArgument("IfList", ifList.ToArray()));
                }

                return arguments.ToArray();
            }
            catch (Exception e)
            {
                throw new ArgumentEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
