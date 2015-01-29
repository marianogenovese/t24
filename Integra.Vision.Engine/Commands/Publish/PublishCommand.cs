//-----------------------------------------------------------------------
// <copyright file="PublishCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Event;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for publish command.
    /// </summary>
    internal sealed class PublishCommand : CommandBase
    {
        /// <summary>
        /// Execution plan node
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Event received from the remote client
        /// </summary>
        private readonly EventObject eventObject;

        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator;

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        /// <param name="eventObject">Incoming event from the remote client</param>
        public PublishCommand(PlanNode node, EventObject eventObject) : base(node)
        {
            this.node = node;
            this.eventObject = eventObject;
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.Publish;
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
        /// Gets the event receive from the remote client
        /// </summary>
        public EventObject Event
        {
            get
            {
                return this.eventObject;
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
                    this.dependencyEnumerator = new PublishDependencyEnumerator(this.node);
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
                    this.argumentEnumerator = new PublishArgumentEnumerator(this.node);
                }

                return this.argumentEnumerator;
            }
        }
    }
}
