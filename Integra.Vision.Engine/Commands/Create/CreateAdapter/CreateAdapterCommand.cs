//-----------------------------------------------------------------------
// <copyright file="CreateAdapterCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Language;
 
    /// <summary>
    /// Base class for create adapters
    /// </summary>
    internal sealed class CreateAdapterCommand : CreateObjectCommandBase
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
        /// Initializes a new instance of the <see cref="CreateAdapterCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public CreateAdapterCommand(PlanNode node) : base(node)
        {
            this.node = node;
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.CreateAdapter;
            }
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

        /// <summary>
        /// Gets the adapter type: input/output
        /// </summary>
        public AdapterTypeEnum AdapterType
        {
            get
            {
                return (AdapterTypeEnum)this.Arguments["Type"].Value;
            }
        }

        /// <summary>
        /// Gets the parameters of the adapter
        /// </summary>
        public List<PlanNode> Parameters
        {
            get
            {
                return (List<PlanNode>)this.Arguments["Parameters"].Value;
            }
        }

        /// <summary>
        /// Gets the assembly name
        /// </summary>
        public string AssemblyName
        {
            get
            {
                return this.Arguments["AssemblyName"].Value.ToString();
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
                    this.dependencyEnumerator = new CreateAdapterDependencyEnumerator(this.node);
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
                    this.argumentEnumerator = new CreateAdapterArgumentEnumerator(this.node);
                }

                return this.argumentEnumerator;
            }
        }
    }
}
