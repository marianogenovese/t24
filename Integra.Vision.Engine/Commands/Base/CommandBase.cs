//-----------------------------------------------------------------------
// <copyright file="CommandBase.cs" company="Integra.Vision.Engine">
// Doc goes here
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    
    /// <summary>
    /// CommandBase
    /// Base class of command, have common methods and properties
    /// </summary>
    internal abstract class CommandBase
    {
        /// <summary>
        /// type
        /// Indicate what type of command is
        /// </summary>
        private CommandTypeEnum type;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class
        /// </summary>
        /// <param name="type">Doc goes here</param>
        public CommandBase(CommandTypeEnum type)
        {
            this.type = type;
        }

        /// <summary>
        /// Gets Type
        /// Indicate what type of command is
        /// </summary>
        public CommandTypeEnum Type
        {
            get
            {
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