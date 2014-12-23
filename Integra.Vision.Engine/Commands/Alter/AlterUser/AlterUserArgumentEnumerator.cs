//-----------------------------------------------------------------------
// <copyright file="AlterUserArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Alter.AlterUser
{
    /// <summary>
    /// Contains argument enumerator logic for alter user command
    /// </summary>
    internal sealed class AlterUserArgumentEnumerator : IArgumentEnumerator
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
                    arguments.Add(new CommandArgument("Password", interpretedCommand.Plan.Root.Children[1].Properties["Value"].ToString()));
                    arguments.Add(new CommandArgument("Status", interpretedCommand.Plan.Root.Children[2].Properties["Status"].ToString()));

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
