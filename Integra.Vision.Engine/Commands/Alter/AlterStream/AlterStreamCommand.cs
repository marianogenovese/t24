//-----------------------------------------------------------------------
// <copyright file="AlterStreamCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Alter.AlterStream
{
    using System;
    using Integra.Vision.Engine.Commands.Create.CreateStream;
    using Integra.Vision.Engine.Commands.Drop.DropStream;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for alter streams
    /// </summary>
    internal sealed class AlterStreamCommand : AlterObjectCommandBase<CreateStreamCommand, DropStreamCommand>
    {
        /// <summary>
        /// Execution plan node
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new AlterStreamArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new AlterStreamDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterStreamCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterStreamCommand(PlanNode node) : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.AlterStream;
            }
        }

        /// <summary>
        /// Gets command dependency enumerator
        /// </summary>
        protected override IDependencyEnumerator DependencyEnumerator
        {
            get
            {
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
                return this.argumentEnumerator;
            }
        }

        /// <summary>
        /// Contains alter stream logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // drop the especified stream
            DropObject dropStream = new DropObject(this.Arguments, this.Dependencies);
            dropStream.Execute();

            // create the new stream
            CreateObject createStream = new CreateObject(this.Arguments, this.Dependencies);
            createStream.Execute();
        }

        /// <summary>
        /// class for create a stream without chain execution
        /// </summary>
        private class CreateObject : CreateStreamCommand
        {
            /// <summary>
            /// Argument enumerator implementation for this command
            /// </summary>
            private IReadOnlyNamedElementCollection<CommandArgument> arguments;

            /// <summary>
            /// Dependency enumerator implementation for this command
            /// </summary>
            private IReadOnlyNamedElementCollection<CommandDependency> dependencies;

            /// <summary>
            /// Initializes a new instance of the <see cref="CreateObject"/> class
            /// </summary>
            /// <param name="arguments">alter command arguments</param>
            /// /// <param name="dependencies">alter command dependencies</param>
            public CreateObject(IReadOnlyNamedElementCollection<CommandArgument> arguments, IReadOnlyNamedElementCollection<CommandDependency> dependencies)
                : base(null)
            {
                this.arguments = arguments;
                this.dependencies = dependencies;
            }

            /// <summary>
            /// Gets the alter command arguments passed to this class
            /// </summary>
            protected override IReadOnlyNamedElementCollection<CommandArgument> Arguments
            {
                get
                {
                    return this.arguments;
                }
            }

            /// <summary>
            /// Gets the alter command dependencies passed to this class
            /// </summary>
            protected override IReadOnlyNamedElementCollection<CommandDependency> Dependencies
            {
                get
                {
                    return this.dependencies;
                }
            }

            /// <summary>
            /// Save command arguments
            /// </summary>
            protected override void OnExecute()
            {
                this.SaveArguments();
            }
        }

        /// <summary>
        /// class for drop a stream without chain execution
        /// </summary>
        private class DropObject : DropStreamCommand
        {
            /// <summary>
            /// Argument enumerator implementation for this command
            /// </summary>
            private IReadOnlyNamedElementCollection<CommandArgument> arguments;

            /// <summary>
            /// Dependency enumerator implementation for this command
            /// </summary>
            private IReadOnlyNamedElementCollection<CommandDependency> dependencies;

            /// <summary>
            /// Initializes a new instance of the <see cref="DropObject"/> class
            /// </summary>
            /// <param name="arguments">alter command arguments</param>
            /// /// <param name="dependencies">alter command dependencies</param>
            public DropObject(IReadOnlyNamedElementCollection<CommandArgument> arguments, IReadOnlyNamedElementCollection<CommandDependency> dependencies)
                : base(new PlanNode())
            {
                this.arguments = arguments;
                this.dependencies = dependencies;
            }

            /// <summary>
            /// Gets the alter command arguments passed to this class
            /// </summary>
            protected override IReadOnlyNamedElementCollection<CommandArgument> Arguments
            {
                get
                {
                    return this.arguments;
                }
            }

            /// <summary>
            /// Gets the alter command dependencies passed to this class
            /// </summary>
            protected override IReadOnlyNamedElementCollection<CommandDependency> Dependencies
            {
                get
                {
                    return this.dependencies;
                }
            }

            /// <summary>
            /// Save command arguments
            /// </summary>
            protected override void OnExecute()
            {
                this.DeleteArguments();
            }
        }
    }
}
