//-----------------------------------------------------------------------
// <copyright file="CreateStreamArgumentEnumerator.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateStream
{
    /// <summary>
    /// Contains argument enumerator logic for Create Stream command
    /// </summary>
    internal sealed class CreateStreamArgumentEnumerator : IArgumentEnumerator
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

                    System.Collections.Generic.List<System.Tuple<string, string>> projectionList = new System.Collections.Generic.List<System.Tuple<string, string>>();

                    int childrenCount = interpretedCommand.Plan.Root.Children.Count;
                    if (childrenCount == 4)
                    {
                        arguments.Add(new CommandArgument("From", interpretedCommand.Plan.Root.Children[1].Children[0].Properties["Value"].ToString()));
                        arguments.Add(new CommandArgument("Where", interpretedCommand.Plan.Root.Children[2].Children[0].NodeText));

                        foreach (Integra.Vision.Language.PlanNode plan in interpretedCommand.Plan.Root.Children[3].Children)
                        {
                            projectionList.Add(new System.Tuple<string, string>(plan.Children[0].Properties["Value"].ToString(), plan.Children[1].NodeText));
                        }

                        arguments.Add(new CommandArgument("Select", projectionList));
                    }
                    else if (childrenCount == 7)
                    {
                        arguments.Add(new CommandArgument("JoinSourceName", interpretedCommand.Plan.Root.Children[1].Children[0].Properties["Value"]));
                        arguments.Add(new CommandArgument("JoinSourceAlias", interpretedCommand.Plan.Root.Children[1].Children[1].Properties["Value"]));
                        arguments.Add(new CommandArgument("WithSourceName", interpretedCommand.Plan.Root.Children[2].Children[0].Properties["Value"]));
                        arguments.Add(new CommandArgument("WithSourceAlias", interpretedCommand.Plan.Root.Children[2].Children[1].Properties["Value"]));
                        arguments.Add(new CommandArgument("On", interpretedCommand.Plan.Root.Children[3].Children[0].NodeText));
                        arguments.Add(new CommandArgument("ApplyWindow", interpretedCommand.Plan.Root.Children[4].Children[0].Properties["Value"]));
                        arguments.Add(new CommandArgument("Where", interpretedCommand.Plan.Root.Children[5].Children[0].NodeText));

                        foreach (Integra.Vision.Language.PlanNode plan in interpretedCommand.Plan.Root.Children[6].Children)
                        {
                            projectionList.Add(new System.Tuple<string, string>(plan.Children[0].Properties["Value"].ToString(), plan.Children[1].NodeText));
                        }

                        arguments.Add(new CommandArgument("Select", projectionList));
                    }

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
