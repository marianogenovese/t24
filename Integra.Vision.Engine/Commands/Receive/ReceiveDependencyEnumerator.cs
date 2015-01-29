//-----------------------------------------------------------------------
// <copyright file="ReceiveDependencyEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Contains dependency enumerator logic for Receive command
    /// </summary>
    internal sealed class ReceiveDependencyEnumerator : IDependencyEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceiveDependencyEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public ReceiveDependencyEnumerator(PlanNode node)
        {
            this.node = node;
        }

        /// <summary>
        /// Dependency enumeration implementation
        /// </summary>
        /// <param name="command">Command in context used for enumerate their arguments</param>
        /// <returns>Collection of command arguments</returns>
        public CommandDependency[] Enumerate(CommandBase command)
        {
            try
            {
                return new CommandDependency[] { };
            }
            catch (Exception e)
            {
                throw new DependencyEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
