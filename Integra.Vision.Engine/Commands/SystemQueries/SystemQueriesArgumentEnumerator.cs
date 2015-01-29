//-----------------------------------------------------------------------
// <copyright file="SystemQueriesArgumentEnumerator.cs" company="Integra.Vision.Engine">
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
    internal sealed class SystemQueriesArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemQueriesArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public SystemQueriesArgumentEnumerator(PlanNode node)
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
                arguments.Add(new CommandArgument("From", this.node.Properties["From"].ToString()));

                if (this.node.Properties["Where"] != null)
                {
                    arguments.Add(new CommandArgument("Where", this.node.Properties["Where"].ToString()));
                }
                else
                {
                    arguments.Add(new CommandArgument("Where", string.Empty));
                }

                arguments.Add(new CommandArgument("Select", this.node.Properties["Select"].ToString()));
        
                return arguments.ToArray();
            }
            catch (Exception e)
            {
                throw new ArgumentEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
