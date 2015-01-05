//-----------------------------------------------------------------------
// <copyright file="DenyPermissionArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Permission.Deny
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Contains argument enumerator logic for deny permission command
    /// </summary>
    internal sealed class DenyPermissionArgumentEnumerator : IArgumentEnumerator
    {
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
                /*
                arguments.Add(new CommandArgument("SecureObjectType", interpretedCommand.Plan.Root.Properties["SecureObjectType"].ToString()));
                arguments.Add(new CommandArgument("SecureObjectName", interpretedCommand.Plan.Root.Children[0].Properties["Value"].ToString()));
                arguments.Add(new CommandArgument("AsignableObjectType", interpretedCommand.Plan.Root.Children[1].Properties["To"].ToString()));
                arguments.Add(new CommandArgument("AsignableObjectName", interpretedCommand.Plan.Root.Children[1].Children[0].Properties["Value"].ToString()));
                */
                return arguments.ToArray();
            }
            catch (Exception e)
            {
                throw new ArgumentEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
