//-----------------------------------------------------------------------
// <copyright file="AlterUserArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Contains argument enumerator logic for alter user command
    /// </summary>
    internal sealed class AlterUserArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node
        /// </summary>
        private readonly PlanNode node;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AlterUserArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterUserArgumentEnumerator(PlanNode node)
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
                arguments.Add(new CommandArgument("Password", this.node.Children[1].Properties["Value"].ToString()));

                if (this.node.Children[2].Properties["Status"].ToString().Equals(UserStatusEnum.Enable.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("Status", UserStatusEnum.Enable));
                }
                else
                {
                    arguments.Add(new CommandArgument("Status", UserStatusEnum.Disable));
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
