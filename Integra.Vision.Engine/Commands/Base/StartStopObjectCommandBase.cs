﻿//-----------------------------------------------------------------------
// <copyright file="StartStopObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for start or stop objects
    /// </summary>
    internal abstract class StartStopObjectCommandBase : UserDefinedObjectCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartStopObjectCommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public StartStopObjectCommandBase(PlanNode node) : base(node)
        {
        }
    }
}