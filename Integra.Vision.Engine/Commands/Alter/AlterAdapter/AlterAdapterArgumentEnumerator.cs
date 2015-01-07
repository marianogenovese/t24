//-----------------------------------------------------------------------
// <copyright file="AlterAdapterArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;

    /// <summary>
    /// Contains argument enumerator logic for alter adapter command
    /// </summary>
    internal sealed class AlterAdapterArgumentEnumerator : IArgumentEnumerator
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
                arguments.Add(new CommandArgument("Name", interpretedCommand.Plan.Root.Children[0].Properties["Value"].ToString()));
                arguments.Add(new CommandArgument("Type", interpretedCommand.Plan.Root.Children[1].Properties["Value"].ToString()));

                System.Collections.Generic.List<object> parametersList = new System.Collections.Generic.List<object>();
                foreach (PlanNode child in interpretedCommand.Plan.Root.Children[2].Children)
                {
                    parametersList.Add(child);
                }

                arguments.Add(new CommandArgument("Parameters", parametersList));
                arguments.Add(new CommandArgument("AssemblyName", interpretedCommand.Plan.Root.Children[3].Properties["Value"].ToString()));
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
