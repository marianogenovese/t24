//-----------------------------------------------------------------------
// <copyright file="SetTraceCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for trace objects
    /// </summary>
    internal class SetTraceCommand : SetCommandBase
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
        /// Initializes a new instance of the <see cref="SetTraceCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public SetTraceCommand(PlanNode node) : base(node)
        {
            this.node = node;
        }
        
        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets the trace level
        /// </summary>
        public int Level
        {
            get
            {
                return (int)this.Arguments["Level"].Value;
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
                    this.argumentEnumerator = new SetTraceArgumentEnumerator(this.node);
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
                    this.dependencyEnumerator = new SetTraceDependencyEnumerator(this.node);
                }

                return this.dependencyEnumerator;
            }
        }
    }
}
