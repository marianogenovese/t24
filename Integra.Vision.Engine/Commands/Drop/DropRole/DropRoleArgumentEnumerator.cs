//-----------------------------------------------------------------------
// <copyright file="DropRoleArgumentEnumerator.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Drop.DropRole
{
    /// <summary>
    /// Contains argument enumerator logic for Drop Role command
    /// </summary>
    internal sealed class DropRoleArgumentEnumerator : IArgumentEnumerator
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
