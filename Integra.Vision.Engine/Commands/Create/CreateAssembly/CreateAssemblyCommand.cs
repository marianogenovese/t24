//-----------------------------------------------------------------------
// <copyright file="CreateAssemblyCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Base class for create assemblies
    /// </summary>
    internal sealed class CreateAssemblyCommand : CreateObjectCommandBase
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
        /// Initializes a new instance of the <see cref="CreateAssemblyCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public CreateAssemblyCommand(PlanNode node) : base(node)
        {
            this.node = node;
        }

        /// <summary>
        /// Gets command argument enumerator
        /// </summary>
        protected override IArgumentEnumerator ArgumentEnumerator
        {
            get
            {
                if(this.argumentEnumerator == null)
                {
                    this.argumentEnumerator = new CreateAssemblyArgumentEnumerator(this.node);
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
                if(this.dependencyEnumerator == null)
                {
                    this.dependencyEnumerator = new CreateAssemblyDependencyEnumerator(this.node);
                }

                return this.dependencyEnumerator;
            }
        }
    }
}
