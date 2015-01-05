//-----------------------------------------------------------------------
// <copyright file="ReferenceableObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for reference object
    /// </summary>
    internal abstract class ReferenceableObjectCommandBase : IdentifiableUserDefinedObjectCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceableObjectCommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public ReferenceableObjectCommandBase(PlanNode node) : base(node)
        {
        }
    }
}
