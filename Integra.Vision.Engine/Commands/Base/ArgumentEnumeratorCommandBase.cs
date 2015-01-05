//-----------------------------------------------------------------------
// <copyright file="ArgumentEnumeratorCommandBase.cs" company="Integra.Vision">
//     Copyright (c) Integra.Vision. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using Integra.Vision.Engine.Resources;
    using Integra.Vision.Language;
    
    /// <summary>
    /// ArgumentEnumeratorCommandBase
    /// Encapsulate argument enumeration logic
    /// </summary>
    internal abstract class ArgumentEnumeratorCommandBase : CommandBase
    {
        /// <summary>
        /// Contains a collection of arguments 
        /// </summary>
        private CommandArgumentCollection argumentCollection = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentEnumeratorCommandBase"/> class
        /// </summary>
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public ArgumentEnumeratorCommandBase(PlanNode node) : base(node)
        {
        }

        /// <summary>
        /// Gets command arguments
        /// </summary>
        protected virtual IReadOnlyNamedElementCollection<CommandArgument> Arguments
        {
            get
            {
                if (this.argumentCollection == null)
                {
                    throw new ArgumentEnumerationException(Resources.SR.InvalidCommandElementCollection);
                }

                return this.argumentCollection;
            }
        }

        /// <summary>
        /// Gets an instance of argument enumerator
        /// </summary>
        protected abstract IArgumentEnumerator ArgumentEnumerator
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
                throw new ArgumentEnumerationException(Integra.Vision.Engine.SR.EnumeratorNotImplemented);
            }

            try
            {
                CommandArgument[] arguments = this.ArgumentEnumerator.Enumerate(this);
                this.argumentCollection = new CommandArgumentCollection(arguments);
            }
            catch (Exception e)
            {
                throw new ArgumentEnumerationException(Integra.Vision.Engine.SR.EnumerationException, e);
            }
        }
    }
}
