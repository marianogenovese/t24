//-----------------------------------------------------------------------
// <copyright file="BootEngineArgumentEnumerator.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;

    /// <summary>
    /// Contains argument enumerator logic for Boot Engine command
    /// </summary>
    internal sealed class BootEngineArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="BootEngineArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public BootEngineArgumentEnumerator(PlanNode node)
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
                arguments.Add(new CommandArgument("Action", this.node.Properties["Action"].ToString()));

                if (ObjectTypeEnum.Engine.ToString().Equals(this.node.Properties["ObjectType"].ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("ObjectType", ObjectTypeEnum.Engine));
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
