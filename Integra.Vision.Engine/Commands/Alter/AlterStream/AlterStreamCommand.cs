//-----------------------------------------------------------------------
// <copyright file="AlterStreamCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for alter streams
    /// </summary>
    internal sealed class AlterStreamCommand : AlterObjectCommandBase
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
        /// Initializes a new instance of the <see cref="AlterStreamCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterStreamCommand(PlanNode node) : base(node)
        {
            this.node = node;
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
        /// Gets source name
        /// </summary>
        public string From
        {
            get
            {
                return this.Arguments["From"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets the projection list
        /// </summary>
        public Tuple<string, string>[] Select
        {
            get
            {
                return (Tuple<string, string>[])this.Arguments["Select"].Value;
            }
        }

        /// <summary>
        /// Gets the stream conditions
        /// </summary>
        public string Where
        {
            get
            {
                return this.Arguments["Where"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets join source name
        /// </summary>
        public string JoinSourceName
        {
            get
            {
                return this.Arguments["JoinSourceName"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets join source alias
        /// </summary>
        public string JoinSourceAlias
        {
            get
            {
                return this.Arguments["JoinSourceAlias"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets with source name
        /// </summary>
        public string WithSourceName
        {
            get
            {
                return this.Arguments["WithSourceName"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets with source alias
        /// </summary>
        public string WithSourceAlias
        {
            get
            {
                return this.Arguments["WithSourceAlias"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets on condition
        /// </summary>
        public string On
        {
            get
            {
                return this.Arguments["On"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets apply window statement
        /// </summary>
        public string ApplyWindow
        {
            get
            {
                return this.Arguments["ApplyWindow"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets a value indicating whether is a simple stream
        /// </summary>
        public bool IsSimpleStream
        {
            get
            {
                return (bool)this.Arguments["IsSimpleStream"].Value;
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
                    this.dependencyEnumerator = new AlterStreamDependencyEnumerator(this.node);
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
                    this.argumentEnumerator = new AlterStreamArgumentEnumerator(this.node);
                }

                return this.argumentEnumerator;
            }
        }
    }
}
