//-----------------------------------------------------------------------
// <copyright file="SecureContextCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    /// <summary>
    /// SecureContextCommandBase
    /// Encapsulate command security logic
    /// </summary>
    internal abstract class SecureContextCommandBase : InterpretedCommandBase
    {
        /// <summary>
        /// Security context
        /// </summary>
        private ISecurityContext securityContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureContextCommandBase"/> class
        /// </summary>
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public SecureContextCommandBase(CommandTypeEnum commandType, string commandText, ISecurityContext securityContext) : base(commandType, commandText)
        {
            this.securityContext = securityContext;
        }
        
        /// <summary>
        /// Gets the security context
        /// </summary>
        protected ISecurityContext SecurityContext
        {
            get
            {
                return this.securityContext;
            }
        }
    }
}
