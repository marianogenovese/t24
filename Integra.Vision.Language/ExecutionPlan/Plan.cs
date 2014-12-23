//-----------------------------------------------------------------------
// <copyright file="Plan.cs" company="Integra.Vision.Common">
//     Copyright (c) Integra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language
{
    /// <summary>
    /// Plan class
    /// </summary>
    internal sealed class Plan
    {
        /// <summary>
        /// root node of the execution plan
        /// </summary>
        private PlanNode root;

        /// <summary>
        /// Gets or sets root node of the execution plan
        /// </summary>
        public PlanNode Root { get; set; }
    }
}
