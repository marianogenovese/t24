//-----------------------------------------------------------------------
// <copyright file="SetTraceArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Trace
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Contains argument enumerator logic for set trace command
    /// </summary>
    internal sealed class SetTraceArgumentEnumerator : IArgumentEnumerator
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
                arguments.Add(new CommandArgument("Level", interpretedCommand.Plan.Root.Children[0].Properties["Value"].ToString()));
                arguments.Add(new CommandArgument("ObjectOrFamily", interpretedCommand.Plan.Root.Properties["ObjectToTrace"].ToString()));
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
