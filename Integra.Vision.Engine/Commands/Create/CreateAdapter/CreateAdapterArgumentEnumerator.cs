//-----------------------------------------------------------------------
// <copyright file="CreateAdapterArgumentEnumerator.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateAdapter
{
    using Integra.Vision.Language;

    /// <summary>
    /// Contains argument enumerator logic for Create Adapter command
    /// </summary>
    internal sealed class CreateAdapterArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Argument enumeration implementation
        /// </summary>
        /// <param name="command">Command in context used for enumerate their arguments</param>
        /// <returns>Collection of command arguments</returns>
        public CommandArgument[] Enumerate(CommandBase command)
        {
            if (command is InterpretedCommandBase)
            {
                System.Collections.Generic.List<CommandArgument> arguments = new System.Collections.Generic.List<CommandArgument>();
                InterpretedCommandBase interpretedCommand = command as InterpretedCommandBase;

                try
                {
                    arguments.Add(new CommandArgument("Name", interpretedCommand.Plan.Root.Children[0].Properties["Value"].ToString()));
                    arguments.Add(new CommandArgument("Type", interpretedCommand.Plan.Root.Children[1].Properties["Value"].ToString()));

                    System.Collections.Generic.List<object> parametersList = new System.Collections.Generic.List<object>();
                    foreach (PlanNode child in interpretedCommand.Plan.Root.Children[2].Children)
                    {
                        parametersList.Add(child);
                    }

                    arguments.Add(new CommandArgument("Parameters", parametersList));
                    arguments.Add(new CommandArgument("AssemblyName", interpretedCommand.Plan.Root.Children[3].Properties["Value"].ToString()));
                    return arguments.ToArray();
                }
                catch (System.Exception e)
                {
                    throw new ArgumentEnumerationException(Resources.SR.EnumerationException, e);
                }
            }

            return new CommandArgument[] { };
        }
    }
}
