//-----------------------------------------------------------------------
// <copyright file="IdentifiableUserDefinedObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    /// <summary>
    /// Base class for identifiable user defined objects
    /// </summary>
    internal abstract class IdentifiableUserDefinedObjectCommandBase : UserDefinedObjectCommandBase
    {
        /// <summary>
        /// Id of user defined object
        /// </summary>
        private string id = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableUserDefinedObjectCommandBase"/> class
        /// </summary>
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public IdentifiableUserDefinedObjectCommandBase(CommandTypeEnum commandType, string commandText, ISecurityContext securityContext) : base(commandType, commandText, securityContext)
        {
        }

        /// <summary>
        /// Gets the Id of user defined object in current command
        /// </summary>
        public string Id
        {
            get
            {
                return this.id;
            }
        }
    }
}
