//-----------------------------------------------------------------------
// <copyright file="DependencyEnumeratorCommandBase.cs" company="Integra.Vision">
//     Copyright (c) Integra.Vision. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using Integra.Vision.Language;

    /// <summary>
    /// DependencyEnumeratorCommandBase
    /// Encapsulate dependency enumeration logic
    /// </summary>
    internal abstract class DependencyEnumeratorCommandBase : ArgumentEnumeratorCommandBase
    {
        /// <summary>
        /// Contains a collection of dependencies 
        /// </summary>
        private CommandDependencyCollection dependencyCollection = null;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyEnumeratorCommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public DependencyEnumeratorCommandBase(PlanNode node) : base(node)
        {
        }

        /// <summary>
        /// Gets command dependencies
        /// </summary>
        protected virtual IReadOnlyNamedElementCollection<CommandDependency> Dependencies
        {
            get
            {
                if (this.dependencyCollection == null)
                {
                    throw new DependencyEnumerationException(Resources.SR.InvalidCommandElementCollection);
                }

                return this.dependencyCollection;
            }
        }

        /// <summary>
        /// Gets an instance of dependency enumerator
        /// </summary>
        protected abstract IDependencyEnumerator DependencyEnumerator
        {
            get;
        }

        /// <summary>
        /// Contains logic for enumerate arguments
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            if (this.ArgumentEnumerator == null)
            {
                throw new DependencyEnumerationException(Resources.SR.EnumeratorNotImplemented);
            }

            try
            {
                CommandDependency[] arguments = this.DependencyEnumerator.Enumerate(this);
                this.dependencyCollection = new CommandDependencyCollection(arguments);
            }
            catch (Exception e)
            {
                throw new DependencyEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
