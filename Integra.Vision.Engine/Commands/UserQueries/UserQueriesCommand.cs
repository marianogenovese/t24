//-----------------------------------------------------------------------
// <copyright file="UserQueriesCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System.Linq;
    using System.Linq.Dynamic;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Models.SystemViews;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for system queries
    /// </summary>
    internal sealed class UserQueriesCommand : CommandBase
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
        /// Initializes a new instance of the <see cref="UserQueriesCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public UserQueriesCommand(PlanNode node) : base(node)
        {
            this.node = node;
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.UserQuery;
            }
        }

        /// <summary>
        /// Gets the from statement of the query
        /// </summary>
        public string From
        {
            get
            {
                return this.Arguments["From"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets the where statement of the query
        /// </summary>
        public PlanNode Where
        {
            get
            {
                return (PlanNode)this.Arguments["Where"].Value;
            }
        }

        /// <summary>
        /// Gets the select statement of the query
        /// </summary>
        public PlanNode Select
        {
            get
            {
                return (PlanNode)this.Arguments["Select"].Value;
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
                    this.argumentEnumerator = new SystemQueriesArgumentEnumerator(this.node);
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
                    this.dependencyEnumerator = new SystemQueriesDependencyEnumerator(this.node);
                }

                return this.dependencyEnumerator;
            }
        }
    }
}
