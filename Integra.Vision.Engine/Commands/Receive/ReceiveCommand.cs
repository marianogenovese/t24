//-----------------------------------------------------------------------
// <copyright file="ReceiveCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for boot receive command.
    /// </summary>
    internal sealed class ReceiveCommand : CommandBase
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
        /// Initializes a new instance of the <see cref="ReceiveCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public ReceiveCommand(PlanNode node)
            : base(node)
        {
            this.node = node;
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.Receive;
            }
        }

        /// <summary>
        /// Gets the source name from which events are received
        /// </summary>
        public string SourceName
        {
            get
            {
                return this.Arguments["SourceName"].Value.ToString();
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
                    this.dependencyEnumerator = new ReceiveDependencyEnumerator(this.node);
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
                    this.argumentEnumerator = new ReceiveArgumentEnumerator(this.node);
                }

                return this.argumentEnumerator;
            }
        }
    }
}
