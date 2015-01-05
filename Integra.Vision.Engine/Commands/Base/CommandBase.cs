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