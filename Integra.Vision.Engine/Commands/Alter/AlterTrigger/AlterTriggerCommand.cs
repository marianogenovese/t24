//-----------------------------------------------------------------------
// <copyright file="AlterTriggerCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Alter.AlterTrigger
{
    using System;
    using Integra.Vision.Engine.Commands.Create.CreateTrigger;
    using Integra.Vision.Engine.Commands.Drop.DropTrigger;

    /// <summary>
    /// Base class for alter triggers
    /// </summary>
    internal sealed class AlterTriggerCommand : AlterObjectCommandBase<CreateTriggerCommand, DropTriggerCommand>
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new AlterTriggerArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new AlterTriggerDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterTriggerCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public AlterTriggerCommand(string commandText, ISecurityContext securityContext) : base(CommandTypeEnum.AlterTrigger, commandText, securityContext)
        {
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
        /// Contains alter trigger logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // drop the especified trigger
            DropObject dropTrigger = new DropObject(this.Arguments, this.Dependencies);
            dropTrigger.Execute();

            // create the new trigger
            CreateObject createTrigger = new CreateObject(this.Arguments, this.Dependencies);
            createTrigger.Execute();
        }

        /// <summary>
        /// class for create a trigger without chain execution
        /// </summary>
        private class CreateObject : CreateTriggerCommand
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
                : base(string.Empty, null)
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
        /// class for drop a trigger without chain execution
        /// </summary>
        private class DropObject : DropTriggerCommand
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
                : base(string.Empty, null)
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
