//-----------------------------------------------------------------------
// <copyright file="AlterSourceCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for alter sources
    /// </summary>
    internal sealed class AlterSourceCommand : AlterObjectCommandBase
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
        /// Initializes a new instance of the <see cref="AlterSourceCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterSourceCommand(PlanNode node) : base(node)
        {
            this.node = node;
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.AlterSource;
            }
        }

        /// <summary>
        /// Gets the source name
        /// </summary>
        public string Name
        {
            get
            {
                return this.Arguments["Name"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets the adapter name
        /// </summary>
        public string From
        {
            get
            {
                return this.Arguments["From"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets the source conditions
        /// </summary>
        public string Where
        {
            get
            {
                return this.Arguments["Where"].Value.ToString();
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
                    this.dependencyEnumerator = new AlterSourceDependencyEnumerator(this.node);
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
                    this.argumentEnumerator = new AlterSourceArgumentEnumerator(this.node);
                }

                return this.argumentEnumerator;
            }
        }
    }
}
