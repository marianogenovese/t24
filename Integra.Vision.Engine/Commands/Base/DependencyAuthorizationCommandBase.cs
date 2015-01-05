//-----------------------------------------------------------------------
// <copyright file="DependencyAuthorizationCommandBase.cs" company="Integra.Vision">
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
    internal abstract class DependencyAuthorizationCommandBase : DependencyValidationCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyAuthorizationCommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public DependencyAuthorizationCommandBase(PlanNode node)
            : base(node)
        {
        }
    }
}
