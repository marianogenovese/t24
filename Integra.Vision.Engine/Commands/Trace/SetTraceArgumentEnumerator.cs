//-----------------------------------------------------------------------
// <copyright file="SetTraceArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Contains argument enumerator logic for set trace command
    /// </summary>
    internal sealed class SetTraceArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetTraceArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public SetTraceArgumentEnumerator(PlanNode node)
        {
            this.node = node;
        }

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
                arguments.Add(new CommandArgument("Level", (int)this.node.Children[0].Properties["Value"]));

                if (this.node.Properties["ObjectToTrace"].ToString().Equals(ObjectTypeEnum.Adapter.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("FamilyName", ObjectTypeEnum.Adapter));
                }
                else if (this.node.Properties["ObjectToTrace"].ToString().Equals(ObjectTypeEnum.Source.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("FamilyName", ObjectTypeEnum.Source));
                }
                else if (this.node.Properties["ObjectToTrace"].ToString().Equals(ObjectTypeEnum.Stream.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("FamilyName", ObjectTypeEnum.Stream));
                }
                else if (this.node.Properties["ObjectToTrace"].ToString().Equals(ObjectTypeEnum.Trigger.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("FamilyName", ObjectTypeEnum.Trigger));
                }
                else if (this.node.Properties["ObjectToTrace"].ToString().Equals(ObjectTypeEnum.Engine.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("FamilyName", ObjectTypeEnum.Engine));
                }
                else
                {
                    arguments.Add(new CommandArgument("ObjectName", this.node.Properties["ObjectToTrace"].ToString()));
                }

                return arguments.ToArray();
            }
            catch (Exception e)
            {
                throw new ArgumentEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
