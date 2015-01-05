//-----------------------------------------------------------------------
// <copyright file="SetCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for set trace command
    /// </summary>
    internal abstract class SetCommandBase : PublicCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetCommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public SetCommandBase(PlanNode node) : base(node)
        {
        }
    }
}
