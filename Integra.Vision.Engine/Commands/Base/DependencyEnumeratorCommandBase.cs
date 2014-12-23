//-----------------------------------------------------------------------
// <copyright file="DependencyEnumeratorCommandBase.cs" company="Integra.Vision">
//     Copyright (c) Integra.Vision. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Integra.Vision.Engine.Commands
{
    using System;

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
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public DependencyEnumeratorCommandBase(CommandTypeEnum commandType, string commandText, ISecurityContext securityContext) : base(commandType, commandText, securityContext)
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
                    throw new DependencyEnumerationException(SR.InvalidCommandElementCollection);
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
                throw new DependencyEnumerationException(SR.EnumeratorNotImplemented);
            }

            try
            {
                CommandDependency[] arguments = this.DependencyEnumerator.Enumerate(this);
                this.dependencyCollection = new CommandDependencyCollection(arguments);
            }
            catch (Exception e)
            {
                throw new DependencyEnumerationException(SR.EnumerationException, e);
            }
        }
    }
}
