//-----------------------------------------------------------------------
// <copyright file="PermissionCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for grant, deny and revoke permissions
    /// </summary>
    internal abstract class PermissionCommandBase : PublicCommandBase
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
        /// <param name="node">Execution plan node that have the command arguments</param>
        public PermissionCommandBase(PlanNode node) : base(node)
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
