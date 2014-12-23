//-----------------------------------------------------------------------
// <copyright file="PersistenceContextCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    /// <summary>
    /// Base class for commands with persistence logic
    /// </summary>
    internal abstract class PersistenceContextCommandBase : PublicCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceContextCommandBase"/> class
        /// </summary>
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public PersistenceContextCommandBase(CommandTypeEnum commandType, string commandText, ISecurityContext securityContext) : base(commandType, commandText, securityContext)
        {
        }
    }
}
