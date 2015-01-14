//-----------------------------------------------------------------------
// <copyright file="CreateTriggerCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for create triggers
    /// </summary>
    internal class CreateTriggerCommand : CreateObjectCommandBase
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
        /// Initializes a new instance of the <see cref="CreateTriggerCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public CreateTriggerCommand(PlanNode node) : base(node)
        {
            this.node = node;
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.CreateTrigger;
            }
        }

        /// <summary>
        /// Gets the stream name
        /// </summary>
        public string StreamName
        {
            get
            {
                return this.Arguments["StreamName"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets list of output adapter names
        /// </summary>
        public string[] SendList
        {
            get
            {
                return (string[])this.Arguments["SendList"].Value;
            }
        }

        /// <summary>
        /// Gets trigger window
        /// </summary>
        public string ApplyWindow
        {
            get
            {
                return this.Arguments["ApplyWindow"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets trigger bifurcation list
        /// </summary>
        public Tuple<string, string[]>[] IfList
        {
            get
            {
                return (Tuple<string, string[]>[])this.Arguments["IfList"].Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is a simple stream
        /// </summary>
        public bool IsSimpleTrigger
        {
            get
            {
                return (bool)this.Arguments["IsSimpleTrigger"].Value;
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
                    this.dependencyEnumerator = new CreateTriggerDependencyEnumerator(this.node);
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
                    this.argumentEnumerator = new CreateTriggerArgumentEnumerator(this.node);
                }

                return this.argumentEnumerator;
            }
        }
    }
}
