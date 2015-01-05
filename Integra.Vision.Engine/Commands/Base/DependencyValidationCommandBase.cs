//-----------------------------------------------------------------------
// <copyright file="DependencyValidationCommandBase.cs" company="Integra.Vision">
//     Copyright (c) Integra.Vision. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// DependencyValidationCommandBase
    /// Encapsulate argument validation logic
    /// </summary>
    internal abstract class DependencyValidationCommandBase : ArgumentValidationCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyValidationCommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public DependencyValidationCommandBase(PlanNode node) : base(node)
        {
        }
    }
}
