//-----------------------------------------------------------------------
// <copyright file="BootEngineCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for start an adapter
    /// </summary>
    internal sealed class BootEngineCommand : CommandBase
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
        /// Initializes a new instance of the <see cref="BootEngineCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public BootEngineCommand(PlanNode node)
            : base(node)
        {
            this.node = node;
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get 
            {
                return CommandTypeEnum.Boot;
            }
        }

        /// <summary>
        /// Gets the action to execute
        /// </summary>
        public string Action
        {
            get
            {
                return this.Arguments["Action"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets the object type
        /// </summary>
        public ObjectTypeEnum ObjectType
        {
            get
            {
                return (ObjectTypeEnum)this.Arguments["ObjectType"].Value;
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
                    this.argumentEnumerator = new BootEngineArgumentEnumerator(this.node);
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
                    this.dependencyEnumerator = new BootEngineDependencyEnumerator(this.node);
                }

                return this.dependencyEnumerator;
            }
        }
    }
}
