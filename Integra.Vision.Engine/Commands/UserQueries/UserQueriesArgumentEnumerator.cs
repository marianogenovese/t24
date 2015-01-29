//-----------------------------------------------------------------------
// <copyright file="UserQueriesArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;

    /// <summary>
    /// Contains argument enumerator logic for the system views
    /// </summary>
    internal sealed class UserQueriesArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserQueriesArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public UserQueriesArgumentEnumerator(PlanNode node)
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
                arguments.Add(new CommandArgument("From", this.node.Children[0].Children[0].Properties["Value"].ToString()));

                if (this.node.Children.Count == 3)
                {
                    arguments.Add(new CommandArgument("Where", this.node.Children[1].Children[0]));
                    arguments.Add(new CommandArgument("Select", this.node.Children[2]));
                }
                else
                {
                    arguments.Add(new CommandArgument("Where", null));
                    arguments.Add(new CommandArgument("Select", this.node.Children[1]));
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
