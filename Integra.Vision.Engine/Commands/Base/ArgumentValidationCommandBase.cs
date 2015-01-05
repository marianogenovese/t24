//-----------------------------------------------------------------------
// <copyright file="ArgumentValidationCommandBase.cs" company="Integra.Vision">
//     Copyright (c) Integra.Vision. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// ArgumentValidationCommandBase
    /// Encapsulate argument validation logic
    /// </summary>
    internal abstract class ArgumentValidationCommandBase : DependencyEnumeratorCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentValidationCommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public ArgumentValidationCommandBase(PlanNode node) : base(node)
        {
        }
    }
}
