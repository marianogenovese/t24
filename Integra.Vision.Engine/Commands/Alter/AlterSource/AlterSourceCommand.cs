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
    internal sealed class AlterSourceCommand : AlterObjectCommandBase<CreateSourceCommand, DropSourceCommand>
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new AlterSourceArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new AlterSourceDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterSourceCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterSourceCommand(PlanNode node) : base(node)
        {
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
        /// Contains alter source logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // drop the especified source
            DropObject dropSource = new DropObject(this.Arguments, this.Dependencies);
            dropSource.Execute();

            // create the new source
            CreateObject createSource = new CreateObject(this.Arguments, this.Dependencies);
            createSource.Execute();
        }

        /// <summary>
        /// class for create a source without chain execution
        /// </summary>
        private class CreateObject : CreateSourceCommand
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
                this.SaveArguments();
            }
        }

        /// <summary>
        /// class for drop a source without chain execution
        /// </summary>
        private class DropObject : DropSourceCommand
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
