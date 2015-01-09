//-----------------------------------------------------------------------
// <copyright file="AlterStreamArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Contains argument enumerator logic for alter stream command
    /// </summary>
    internal sealed class AlterStreamArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterStreamArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterStreamArgumentEnumerator(PlanNode node)
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

            try
            {
                arguments.Add(new CommandArgument("Name", this.node.Children[0].Properties["Value"].ToString()));

                System.Collections.Generic.List<System.Tuple<string, string>> projectionList = new System.Collections.Generic.List<System.Tuple<string, string>>();

                int childrenCount = this.node.Children.Count;
                if (childrenCount == 4)
                {
                    arguments.Add(new CommandArgument("IsSimpleStream", true));
                    arguments.Add(new CommandArgument("From", this.node.Children[1].Children[0].Properties["Value"].ToString()));
                    arguments.Add(new CommandArgument("Where", this.node.Children[2].Children[0].NodeText));

                    foreach (Integra.Vision.Language.PlanNode plan in this.node.Children[3].Children)
                    {
                        projectionList.Add(new System.Tuple<string, string>(plan.Children[0].Properties["Value"].ToString(), plan.Children[1].NodeText));
                    }
                }
                else if (childrenCount == 7)
                {
                    arguments.Add(new CommandArgument("IsSimpleStream", false));
                    arguments.Add(new CommandArgument("JoinSourceName", this.node.Children[1].Children[0].Properties["Value"]));
                    arguments.Add(new CommandArgument("JoinSourceAlias", this.node.Children[1].Children[1].Properties["Value"]));
                    arguments.Add(new CommandArgument("WithSourceName", this.node.Children[2].Children[0].Properties["Value"]));
                    arguments.Add(new CommandArgument("WithSourceAlias", this.node.Children[2].Children[1].Properties["Value"]));
                    arguments.Add(new CommandArgument("On", this.node.Children[3].Children[0].NodeText));
                    arguments.Add(new CommandArgument("ApplyWindow", this.node.Children[4].Children[0].Properties["Value"]));
                    arguments.Add(new CommandArgument("Where", this.node.Children[5].Children[0].NodeText));

                    foreach (Integra.Vision.Language.PlanNode plan in this.node.Children[6].Children)
                    {
                        projectionList.Add(new System.Tuple<string, string>(plan.Children[0].Properties["Value"].ToString(), plan.Children[1].NodeText));
                    }
                }

                arguments.Add(new CommandArgument("Select", projectionList.ToArray()));

                return arguments.ToArray();
            }
            catch (Exception e)
            {
                throw new ArgumentEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
