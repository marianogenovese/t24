﻿//-----------------------------------------------------------------------
// <copyright file="StopObjectArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Contains argument enumerator logic for stop a object command
    /// </summary>
    internal sealed class StopObjectArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="StopObjectArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public StopObjectArgumentEnumerator(PlanNode node)
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
                if (this.node.Properties["UserDefinedObject"].ToString().Equals(ObjectTypeEnum.Adapter.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("UserDefinedObject", ObjectTypeEnum.Adapter));
                }
                else if (this.node.Properties["UserDefinedObject"].ToString().Equals(ObjectTypeEnum.Source.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("UserDefinedObject", ObjectTypeEnum.Source));
                }
                else if (this.node.Properties["UserDefinedObject"].ToString().Equals(ObjectTypeEnum.Stream.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("UserDefinedObject", ObjectTypeEnum.Stream));
                }
                else if (this.node.Properties["UserDefinedObject"].ToString().Equals(ObjectTypeEnum.Trigger.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    arguments.Add(new CommandArgument("UserDefinedObject", ObjectTypeEnum.Trigger));
                }

                arguments.Add(new CommandArgument("Name", this.node.Children[0].Properties["Value"].ToString()));

                return arguments.ToArray();
            }
            catch (Exception e)
            {
                throw new ArgumentEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
