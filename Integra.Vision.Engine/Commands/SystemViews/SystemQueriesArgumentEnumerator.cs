//-----------------------------------------------------------------------
// <copyright file="SystemQueriesArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.SystemViews
{
    /// <summary>
    /// Contains argument enumerator logic for the system views
    /// </summary>
    internal sealed class SystemQueriesArgumentEnumerator : IArgumentEnumerator
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
                    arguments.Add(new CommandArgument("from", interpretedCommand.Plan.Root.Properties["from"].ToString()));
                    arguments.Add(new CommandArgument("where", interpretedCommand.Plan.Root.Properties["where"].ToString()));
                    arguments.Add(new CommandArgument("select", interpretedCommand.Plan.Root.Properties["select"].ToString()));

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
