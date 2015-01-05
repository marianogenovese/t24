//-----------------------------------------------------------------------
// <copyright file="DropObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for drop commands of user defined object
    /// </summary>
    internal abstract class DropObjectCommandBase : ReferenceableObjectCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropObjectCommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public DropObjectCommandBase(PlanNode node) : base(node)
        {
        }
    }
}
