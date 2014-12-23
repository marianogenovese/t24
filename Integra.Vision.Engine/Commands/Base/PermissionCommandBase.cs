//-----------------------------------------------------------------------
// <copyright file="PermissionCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;

    /// <summary>
    /// Base class for grant, deny and revoke permissions
    /// </summary>
    internal abstract class PermissionCommandBase : PersistenceContextCommandBase
    {
        /// <summary>
        /// Type of the assignable object
        /// </summary>
        private string assignableObjectType = null;

        /// <summary>
        /// Name of the assignable object
        /// </summary>
        private string assignableObjectName = null;

        /// <summary>
        /// Type of the secure object
        /// </summary>
        private string secureObjectType = null;

        /// <summary>
        /// Name of the secure object
        /// </summary>
        private string secureObjectName = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionCommandBase"/> class
        /// </summary>
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public PermissionCommandBase(CommandTypeEnum commandType, string commandText, ISecurityContext securityContext) : base(commandType, commandText, securityContext)
        {
        }

        /// <summary>
        /// Gets the type of the assignable object
        /// </summary>
        public string AssignableObjectType
        {
            get
            {
                return this.assignableObjectType;
            }
        }

        /// <summary>
        /// Gets the name of the assignable object
        /// </summary>
        public string AssignableObjectName
        {
            get
            {
                return this.assignableObjectName;
            }
        }

        /// <summary>
        /// Gets the type of the secure object
        /// </summary>
        public string SecureObjectType
        {
            get
            {
                return this.secureObjectType;
            }
        }

        /// <summary>
        /// Gets the name of the secure object
        /// </summary>
        public string SecureObjectName
        {
            get
            {
                return this.secureObjectName;
            }
        }
    }
}
