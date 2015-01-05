//-----------------------------------------------------------------------
// <copyright file="IdentifiableUserDefinedObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for identifiable user defined objects
    /// </summary>
    internal abstract class IdentifiableUserDefinedObjectCommandBase : UserDefinedObjectCommandBase
    {
        /// <summary>
        /// Id of user defined object
        /// </summary>
        private string id = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiableUserDefinedObjectCommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public IdentifiableUserDefinedObjectCommandBase(PlanNode node) : base(node)
        {
        }

        /// <summary>
        /// Gets the Id of user defined object in current command
        /// </summary>
        public string Id
        {
            get
            {
                return this.id;
            }
        }
    }
}
