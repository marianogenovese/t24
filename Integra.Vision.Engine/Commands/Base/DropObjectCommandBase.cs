//-----------------------------------------------------------------------
// <copyright file="DropObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    /// <summary>
    /// Base class for drop commands of user defined object
    /// </summary>
    internal abstract class DropObjectCommandBase : ReferenceableObjectCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropObjectCommandBase"/> class
        /// </summary>
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public DropObjectCommandBase(CommandTypeEnum commandType, string commandText, ISecurityContext securityContext) : base(commandType, commandText, securityContext)
        {
        }
    }
}
