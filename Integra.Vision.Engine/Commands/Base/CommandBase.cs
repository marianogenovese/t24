//-----------------------------------------------------------------------
// <copyright file="CommandBase.cs" company="Integra.Vision.Engine">
// Doc goes here
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;
    using System;
    
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
        /// type
        /// Indicate what type of command is
        /// </summary>
        private CommandTypeEnum type;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class
        /// </summary>
        /// <param name="type">Doc goes here</param>
        public CommandBase(PlanNode node)
        {
            this.node = node;
        }

        /// <summary>
        /// Gets Type
        /// Indicate what type of command is
        /// </summary>
        public CommandTypeEnum Type
        {
            get
            {
                // aqui hacer un switch
                return this.type;
            }
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