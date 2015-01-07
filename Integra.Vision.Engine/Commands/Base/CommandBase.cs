//-----------------------------------------------------------------------
// <copyright file="CommandBase.cs" company="Integra.Vision.Engine">
// Doc goes here
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using Integra.Vision.Language;
    
    /// <summary>
    /// CommandBase
    /// Base class of command, have common methods and properties
    /// </summary>
    internal abstract class CommandBase
    {
        /// <summary>
        /// Execution plan node
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public CommandBase(PlanNode node)
        {
            this.node = node;
        }

        /// <summary>
        /// Gets the type of the command.
        /// </summary>
        public abstract CommandTypeEnum Type
        {
            get;
        }

        /// <summary>
        /// Gets command dependencies
        /// </summary>
        public IReadOnlyNamedElementCollection<CommandDependency> Dependencies
        {
            get
            {
                if (this.ArgumentEnumerator == null)
                {
                    throw new DependencyEnumerationException(Resources.SR.EnumeratorNotImplemented);
                }

                try
                {
                    CommandDependency[] arguments = this.DependencyEnumerator.Enumerate(this);
                    return new CommandDependencyCollection(arguments);
                }
                catch (Exception e)
                {
                    throw new DependencyEnumerationException(Resources.SR.EnumerationException, e);
                }
            }
        }

        /// <summary>
        /// Gets the plan node
        /// </summary>
        protected PlanNode Node
        {
            get
            {
                return this.node;
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
        /// Gets command arguments
        /// </summary>
        protected IReadOnlyNamedElementCollection<CommandArgument> Arguments
        {
            get
            {
                if (this.ArgumentEnumerator == null)
                {
                    throw new ArgumentEnumerationException(Integra.Vision.Engine.SR.EnumeratorNotImplemented);
                }

                try
                {
                    CommandArgument[] arguments = this.ArgumentEnumerator.Enumerate(this);
                    return new CommandArgumentCollection(arguments);
                }
                catch (Exception e)
                {
                    throw new ArgumentEnumerationException(Integra.Vision.Engine.SR.EnumerationException, e);
                }
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
        /// Execute command
        /// </summary>
        public void Execute()
        {
            this.OnExecute();
        }
        
        /// <summary>
        /// Contains command specific logic
        /// </summary>
        protected virtual void OnExecute()
        {
            throw new NotImplementedException();
        }
    }
}