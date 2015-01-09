//-----------------------------------------------------------------------
// <copyright file="StartObjectCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for start objects
    /// </summary>
    internal class StartObjectCommand : StartStopObjectCommandBase
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
        /// Initializes a new instance of the <see cref="StartObjectCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public StartObjectCommand(PlanNode node) : base(node)
        {
            this.node = node;
        }
        
        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets the user defined object to start
        /// </summary>
        public ObjectTypeEnum UserDefinedObject
        {
            get
            {
                return (ObjectTypeEnum)this.Arguments["UserDefinedObject"].Value;
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
                    this.argumentEnumerator = new StartObjectArgumentEnumerator(this.node);
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
                    this.dependencyEnumerator = new StartObjectDependencyEnumerator(this.node);
                }

                return this.dependencyEnumerator;
            }
        }
    }
}
