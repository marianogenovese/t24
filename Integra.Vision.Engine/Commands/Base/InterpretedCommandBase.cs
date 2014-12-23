//-----------------------------------------------------------------------
// <copyright file="InterpretedCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Integra.Vision.Language;

    /// <summary>
    /// InterpretedCommandBase
    /// Encapsulate command parsing logic
    /// </summary>
    internal abstract class InterpretedCommandBase : CommandBase
    {
        /// <summary>
        /// Command execution plan
        /// </summary>
        private Plan plan = null;

        /// <summary>
        /// Text that must be interpreted as part of this command
        /// </summary>
        private string commandText = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterpretedCommandBase"/> class
        /// </summary>
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        public InterpretedCommandBase(CommandTypeEnum commandType, string commandText) : base(commandType)
        {
            this.commandText = commandText;
        }

        /// <summary>
        /// Gets command execution plan
        /// </summary>
        public Plan Plan
        {
            get
            {
                if (this.plan == null)
                {
                    throw new InterpretationException(Resources.SR.InvalidPlanAccess);
                }

                return this.plan;
            }
        }
        
        /// <summary>
        /// Implement language parsing logic
        /// </summary>
        protected override void OnExecute()
        {
            // Aqui implementar parseo y construccion de plan
            // no se llama al metodo base porque produciria una excepción
            try
            {
                IntegraParser parser = new IntegraParser(this.commandText);
                this.plan = parser.Parse();
            }
            catch (System.Exception e)
            {
                throw new InterpretationException(e.ToString());
            }
        }
    }
}