//-----------------------------------------------------------------------
// <copyright file="AlterAssemblyCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Base class for alter assemblies
    /// </summary>
    internal sealed class AlterAssemblyCommand : AlterObjectCommandBase
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
        /// Initializes a new instance of the <see cref="AlterAssemblyCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterAssemblyCommand(PlanNode node)
            : base(node)
        {
            this.node = node;
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.AlterAssembly;
            }
        }

        /// <summary>
        /// Gets the assembly name
        /// </summary>
        public string Name
        {
            get
            {
                return this.Arguments["Name"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets the path to the assembly file
        /// </summary>
        public string LocalPath
        {
            get
            {
                return this.Arguments["LocalPath"].Value.ToString();
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
                    this.argumentEnumerator = new AlterAssemblyArgumentEnumerator(this.node);
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
                    this.dependencyEnumerator = new AlterAssemblyDependencyEnumerator(this.node);
                }

                return this.dependencyEnumerator;
            }
        }
    }
}
