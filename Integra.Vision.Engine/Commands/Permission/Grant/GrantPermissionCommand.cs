﻿//-----------------------------------------------------------------------
// <copyright file="GrantPermissionCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for grant permissions
    /// </summary>
    internal sealed class GrantPermissionCommand : CommandBase
    {
        /// <summary>
        /// Execution plan node
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator;

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrantPermissionCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public GrantPermissionCommand(PlanNode node) : base(node)
        {
            this.node = node;
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.Grant;
            }
        }

        /// <summary>
        /// Gets the secure object type
        /// </summary>
        public ObjectTypeEnum SecureObjectType
        {
            get
            {
                return (ObjectTypeEnum)this.Arguments["SecureObjectType"].Value;
            }
        }

        /// <summary>
        /// Gets the secure object name
        /// </summary>
        public string SecureObjectName
        {
            get
            {
                return this.Arguments["SecureObjectName"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets the assignable object type
        /// </summary>
        public ObjectTypeEnum AsignableObjectType
        {
            get
            {
                return (ObjectTypeEnum)this.Arguments["AsignableObjectType"].Value;
            }
        }

        /// <summary>
        /// Gets the assignable object name
        /// </summary>
        public string AssignableObjectName
        {
            get
            {
                return this.Arguments["AsignableObjectName"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets command argument enumerator
        /// </summary>
        protected override IArgumentEnumerator ArgumentEnumerator
        {
            get
            {
                if (this.argumentEnumerator == null)
                {
                    this.argumentEnumerator = new GrantPermissionArgumentEnumerator(this.node);
                }

                return this.argumentEnumerator;
            }
        }

        /// <summary>
        /// Gets command dependency enumerator
        /// </summary>
        protected override IDependencyEnumerator DependencyEnumerator
        {
            get
            {
                if (this.dependencyEnumerator == null)
                {
                    this.dependencyEnumerator = new GrantPermissionDependencyEnumerator(this.node);
                }

                return this.dependencyEnumerator;
            }
        }
    }
}
