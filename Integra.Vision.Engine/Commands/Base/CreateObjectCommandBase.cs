//-----------------------------------------------------------------------
// <copyright file="CreateObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for create commands of user defined object
    /// </summary>
    internal abstract class CreateObjectCommandBase : UserDefinedObjectCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateObjectCommandBase"/> class
        /// </summary>
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public CreateObjectCommandBase(PlanNode node) : base(node)
        {
        }
    }
}