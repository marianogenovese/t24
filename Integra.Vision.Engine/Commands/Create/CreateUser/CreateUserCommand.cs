//-----------------------------------------------------------------------
// <copyright file="CreateUserCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for create users
    /// </summary>
    internal class CreateUserCommand : CreateObjectCommandBase
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
        /// Initializes a new instance of the <see cref="CreateUserCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public CreateUserCommand(PlanNode node) : base(node)
        {
            this.node = node;
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.CreateUser;
            }
        }

        /// <summary>
        /// Gets user password
        /// </summary>
        public string Password
        {
            get
            {
                return this.Arguments["Password"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets user status
        /// </summary>
        public UserStatusEnum Status
        {
            get
            {
                return (UserStatusEnum)this.Arguments["Status"].Value;
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
                    this.dependencyEnumerator = new CreateUserDependencyEnumerator(this.node);
                }

                return this.dependencyEnumerator;
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
                    this.argumentEnumerator = new CreateUserArgumentEnumerator(this.node);
                }

                return this.argumentEnumerator;
            }
        }
    }
}
