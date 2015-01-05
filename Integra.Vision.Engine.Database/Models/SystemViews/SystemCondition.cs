//-----------------------------------------------------------------------
// <copyright file="SystemCondition.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemCondition class
    /// </summary>
    internal sealed class SystemCondition
    {
        /// <summary>
        /// Gets or sets the condition type
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the condition expression
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the object id
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is on condition
        /// </summary>
        public bool IsOnCondition { get; set; }
    }
}
