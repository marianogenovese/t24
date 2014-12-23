//-----------------------------------------------------------------------
// <copyright file="ExclusiveLockCommandBase.cs" company="Integra.Vision">
//     Copyright (c) Integra.Vision. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    /// <summary>
    /// ExclusiveLockCommandBase
    /// Encapsulate exclusive locking use of object for protect command execution
    /// </summary>
    internal abstract class ExclusiveLockCommandBase : DependencyEnumeratorCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExclusiveLockCommandBase"/> class
        /// </summary>
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public ExclusiveLockCommandBase(CommandTypeEnum commandType, string commandText, ISecurityContext securityContext) : base(commandType, commandText, securityContext)
        {
        }
    }
}
