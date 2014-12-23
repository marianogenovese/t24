//-----------------------------------------------------------------------
// <copyright file="StartStopObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    /// <summary>
    /// Base class for start or stop objects
    /// </summary>
    internal abstract class StartStopObjectCommandBase : IdentifiableUserDefinedObjectCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartStopObjectCommandBase"/> class
        /// </summary>
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public StartStopObjectCommandBase(CommandTypeEnum commandType, string commandText, ISecurityContext securityContext) : base(commandType, commandText, securityContext)
        {
        }
    }
}
