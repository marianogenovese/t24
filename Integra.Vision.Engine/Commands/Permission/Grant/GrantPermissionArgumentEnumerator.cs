//-----------------------------------------------------------------------
// <copyright file="GrantPermissionArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;

    /// <summary>
    /// Contains argument enumerator logic for grant permission command
    /// </summary>
    internal sealed class GrantPermissionArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrantPermissionArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public GrantPermissionArgumentEnumerator(PlanNode node)
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
                if (this.node.Properties["SecureObjectType"].ToString().ToLower().Contains(ObjectTypeEnum.Role.ToString().ToLower()))
                {
                    arguments.Add(new CommandArgument("SecureObjectType", ObjectTypeEnum.Role));
                }
                else
                {
                    arguments.Add(new CommandArgument("SecureObjectType", ObjectTypeEnum.User));
                }

                if (this.node.Children[1].Properties["To"].ToString().Equals(ObjectTypeEnum.Role.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("AsignableObjectType", ObjectTypeEnum.Role));
                }
                else
                {
                    arguments.Add(new CommandArgument("AsignableObjectType", ObjectTypeEnum.User));
                }

                arguments.Add(new CommandArgument("SecureObjectName", this.node.Children[0].Properties["Value"].ToString()));
                arguments.Add(new CommandArgument("AsignableObjectName", this.node.Children[1].Children[0].Properties["Value"].ToString()));
                
                return arguments.ToArray();
            }
            catch (Exception e)
            {
                throw new ArgumentEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
