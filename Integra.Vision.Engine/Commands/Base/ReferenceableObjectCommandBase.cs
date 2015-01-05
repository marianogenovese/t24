﻿//-----------------------------------------------------------------------
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
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public ReferenceableObjectCommandBase(PlanNode node) : base(node)
        {
        }
    }
}
