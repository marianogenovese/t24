﻿//-----------------------------------------------------------------------
// <copyright file="AlterSourceArgumentEnumerator.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Contains argument enumerator logic for alter source command
    /// </summary>
    internal sealed class AlterSourceArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterSourceArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterSourceArgumentEnumerator(PlanNode node)
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
                arguments.Add(new CommandArgument("From", this.node.Children[1].Children[0].Properties["Value"].ToString()));
                arguments.Add(new CommandArgument("Where", this.node.Children[2].Children[0].NodeText));

                return arguments.ToArray();
            }
            catch (Exception e)
            {
                throw new ArgumentEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}