//-----------------------------------------------------------------------
// <copyright file="AlterAdapterArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;

    /// <summary>
    /// Contains argument enumerator logic for alter adapter command
    /// </summary>
    internal sealed class AlterAdapterArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterAdapterArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterAdapterArgumentEnumerator(PlanNode node)
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
                arguments.Add(new CommandArgument("Script", "create" + this.node.NodeText.Substring(5)));
                arguments.Add(new CommandArgument("Name", this.node.Children[0].Properties["Value"].ToString()));

                if (this.node.Children[1].Properties["Value"].ToString().Equals(AdapterTypeEnum.Input.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("Type", AdapterTypeEnum.Input));
                }
                else
                {
                    arguments.Add(new CommandArgument("Type", AdapterTypeEnum.Output));
                }

                System.Collections.Generic.List<PlanNode> parametersList = new System.Collections.Generic.List<PlanNode>();
                foreach (PlanNode child in this.node.Children[2].Children)
                {
                    parametersList.Add(child);
                }

                arguments.Add(new CommandArgument("Parameters", parametersList));
                arguments.Add(new CommandArgument("AssemblyName", this.node.Children[3].Properties["Value"].ToString()));
                return arguments.ToArray();
            }
            catch (Exception e)
            {
                throw new ArgumentEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
