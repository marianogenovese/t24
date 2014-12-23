//-----------------------------------------------------------------------
// <copyright file="UserDefinedObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    /// <summary>
    /// Base class for user defined object commands
    /// </summary>
    internal abstract class UserDefinedObjectCommandBase : PersistenceContextCommandBase
    {
        /// <summary>
        /// Name of user defined object
        /// </summary>
        private string name = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDefinedObjectCommandBase"/> class
        /// </summary>
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public UserDefinedObjectCommandBase(CommandTypeEnum commandType, string commandText, ISecurityContext securityContext) : base(commandType, commandText, securityContext)
        {
        }

        /// <summary>
        /// Gets the name of user defined object in current command
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}
