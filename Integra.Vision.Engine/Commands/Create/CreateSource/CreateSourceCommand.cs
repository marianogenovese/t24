//-----------------------------------------------------------------------
// <copyright file="CreateSourceCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for create sources
    /// </summary>
    internal class CreateSourceCommand : CreateObjectCommandBase
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
        /// Initializes a new instance of the <see cref="CreateSourceCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public CreateSourceCommand(PlanNode node) : base(node)
        {
            this.node = node;
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.CreateSource;
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
                    this.dependencyEnumerator = new CreateSourceDependencyEnumerator(this.node);
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
                    this.argumentEnumerator = new CreateSourceArgumentEnumerator(this.node);
                }

                return this.argumentEnumerator;
            }
        }
    }
}
