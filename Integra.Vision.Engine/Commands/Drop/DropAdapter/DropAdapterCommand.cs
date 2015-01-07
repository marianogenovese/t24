﻿//-----------------------------------------------------------------------
// <copyright file="DropAdapterCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for drop adapters
    /// </summary>
    internal class DropAdapterCommand : DropObjectCommandBase
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
        /// Initializes a new instance of the <see cref="DropAdapterCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public DropAdapterCommand(PlanNode node) : base(node)
        {
            this.node = node;
        }

        /// <summary>
        /// Gets the adapter name
        /// </summary>
        public string Name
        {
            get
            {
                return this.Arguments["Name"].Value.ToString();
            }
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.DropAdapter;
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
                    this.argumentEnumerator = new DropAdapterArgumentEnumerator(this.node);
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
                    this.dependencyEnumerator = new DropAssemblyDependencyEnumerator(this.node);
                }

                return this.dependencyEnumerator;
            }
        }
    }
}
