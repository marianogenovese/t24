//-----------------------------------------------------------------------
// <copyright file="CreateUserArgumentEnumerator.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Contains argument enumerator logic for Create User command
    /// </summary>
    internal sealed class CreateUserArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node
        /// </summary>
        private readonly PlanNode node;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public CreateUserArgumentEnumerator(PlanNode node)
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
