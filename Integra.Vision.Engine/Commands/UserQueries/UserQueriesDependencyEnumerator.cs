//-----------------------------------------------------------------------
// <copyright file="UserQueriesDependencyEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language;

    /// <summary>
    /// Contains dependency enumerator logic for the system views
    /// </summary>
    internal sealed class UserQueriesDependencyEnumerator : IDependencyEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserQueriesDependencyEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public UserQueriesDependencyEnumerator(PlanNode node)
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
