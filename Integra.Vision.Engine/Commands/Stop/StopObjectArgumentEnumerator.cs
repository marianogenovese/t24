//-----------------------------------------------------------------------
// <copyright file="StopObjectArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Stop
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Contains argument enumerator logic for stop a object command
    /// </summary>
    internal sealed class StopObjectArgumentEnumerator : IArgumentEnumerator
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
                arguments.Add(new CommandArgument("UserDefinedObject", interpretedCommand.Plan.Root.Properties["UserDefinedObject"].ToString()));
                arguments.Add(new CommandArgument("Name", interpretedCommand.Plan.Root.Children[0].Properties["Value"].ToString()));
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
