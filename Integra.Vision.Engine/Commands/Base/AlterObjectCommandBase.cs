//-----------------------------------------------------------------------
// <copyright file="AlterObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for alter object command
    /// </summary>
    internal abstract class AlterObjectCommandBase : UserDefinedObjectCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlterObjectCommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterObjectCommandBase(PlanNode node) : base(node)
        {
        }
    }
}
