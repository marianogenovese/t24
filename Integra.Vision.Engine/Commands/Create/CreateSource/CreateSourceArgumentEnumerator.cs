//-----------------------------------------------------------------------
// <copyright file="CreateSourceArgumentEnumerator.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateSource
{
    /// <summary>
    /// Contains argument enumerator logic for Create Source command
    /// </summary>
    internal sealed class CreateSourceArgumentEnumerator : IArgumentEnumerator
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
                    arguments.Add(new CommandArgument("From", interpretedCommand.Plan.Root.Children[1].Children[0].Properties["Value"].ToString()));
                    arguments.Add(new CommandArgument("Where", interpretedCommand.Plan.Root.Children[2].Children[0].NodeText));
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
